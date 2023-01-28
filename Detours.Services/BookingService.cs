using System.Text.Json;

using Microsoft.AspNetCore.Http;

using Microsoft.Net.Http.Headers;

using Microsoft.Extensions.Options;

using Microsoft.EntityFrameworkCore;

using Stripe.Checkout;
using Events = Stripe.Events;
using EventUtility = Stripe.EventUtility;
using StripeException = Stripe.StripeException;

using Detours.Core;

using Detours.Mediatr;

using Detours.Data;
using Detours.Data.Options;
using Detours.Data.Entities;
using Detours.Data.Models.Responses;

using Detours.Services.Extensions;
using Detours.Services.Notifications;

namespace Detours.Services;

public class BookingService : IBookingService
{
	private readonly DetoursDbContext _dbContext;
	private readonly IUserService _userService;

	private readonly StrategiesPublisher _publisher;

	private readonly IHttpContextAccessor _httpContextAccessor;

	private readonly SessionService _sessionService;

	private readonly StripeConfiguration _configuration;

	private const string StripeSignatureHeaderName = "Stripe-Signature";

	public BookingService(DetoursDbContext dbContext
		, IUserService userService
		, StrategiesPublisher publisher
		, IHttpContextAccessor httpContextAccessor
		, SessionService sessionService
		, IOptions<StripeConfiguration> options)
	{
		ArgumentNullException.ThrowIfNull(dbContext);
		ArgumentNullException.ThrowIfNull(userService);
		ArgumentNullException.ThrowIfNull(publisher);
		ArgumentNullException.ThrowIfNull(httpContextAccessor);
		ArgumentNullException.ThrowIfNull(sessionService);

		_dbContext = dbContext;
		_userService = userService;
		_publisher = publisher;

		_httpContextAccessor = httpContextAccessor;

		_sessionService = sessionService;

		_configuration = options.GetConfiguration();
	}

	public async Task<ICollection<BookingResponse>> GetMyBookingsAsync(int? page, CancellationToken cancellationToken)
	{
		var currentUser = await _userService.GetCurrentUserAsync(cancellationToken);
		
		var query = _dbContext.Bookings
			.Include(x => x.Tour)
				.ThenInclude(x => x.ImageCover)
			.Include(x => x.Tour)
				.ThenInclude(x => x.StartLocation)
			.Include(x => x.Tour)
				.ThenInclude(x => x.StartDates)
			.Include(x => x.Tour)
				.ThenInclude(x => x.Locations)
			.Include(x => x.Tour)
				.ThenInclude(x => x.Reviews)
			.Where(x => x.UserId == currentUser.Id)
			.OrderBy(x => x.CreatedAt)
			.Select(x => new BookingResponse
			{
				Tour = new QueryTourResponse
				{
					Name = x.Tour.Name,
					Slug = x.Tour.Slug,
					ImageCover = x.Tour.ImageCover.Image,
					Summary = x.Tour.Summary,
					StartLocation = x.Tour.StartLocation.Description,
					Difficulty = x.Tour.Difficulty,
					StartDate = x.Tour.StartDates.OrderBy(x => x.Date).First().Date,
					LocationsCount = x.Tour.Locations.Count,
					MaxGroupSize = x.Tour.MaxGroupSize,
					Price = x.Price,
					RatingsCount = x.Tour.Reviews.Count,
					RatingsAverage = x.Tour.Reviews.Average(x => x.Rating),
				},
				Price = x.Price,
				CreatedAt = x.CreatedAt,
			});

		return await query.ToListAsync(cancellationToken);
	}

	public async Task<SessionCheckoutResponse> GetCheckoutSessionAsync(Guid tourId, CancellationToken cancellationToken)
	{
		var tour = await _dbContext.Tours
			.Include(x => x.ImageCover)
			.FirstOrDefaultAsync(x => x.Id == tourId, cancellationToken);

		if (tour is null)
		{
			throw new ServiceArgumentException($"Tour with id {tourId} does not exist");
		}

		var currentUser = await _userService.GetCurrentUserAsync(cancellationToken);

		var httpContext = _httpContextAccessor.HttpContext;
		var request = httpContext.Request;

		var origin = request.Headers[HeaderNames.Origin];

		var sessionOptions = new SessionCreateOptions
		{
			Mode = "payment",
			PaymentMethodTypes = new List<string> { "card" },
			CustomerEmail = currentUser.Email,
			ClientReferenceId = tourId.ToString(),
			SuccessUrl = $"{origin}/my-bookings?alert=booking",
			CancelUrl = $"{origin}/{tour.Slug}",
			LineItems = new List<SessionLineItemOptions>
			{
				new SessionLineItemOptions
				{
					Quantity = 1,
					PriceData = new SessionLineItemPriceDataOptions
					{
						Currency = "usd",
						UnitAmountDecimal = tour.Price * 100,
						ProductData = new SessionLineItemPriceDataProductDataOptions
						{
							Description = tour.Summary,
							Images = new List<string> { $"{request.Scheme}://{request.Host}/public/img/tours/{tour.ImageCover.Image}" },
							Name = $"{tour.Name} Tour",
						}
					}
				}
			}
		};

		var newSession = await _sessionService.CreateAsync(sessionOptions, cancellationToken: cancellationToken);
		return new SessionCheckoutResponse { Id = newSession.Id };
	}

	public async Task ProcessCheckoutAsync(CancellationToken cancellationToken)
	{
		var request = _httpContextAccessor.HttpContext.Request;

		var stripeSignature = request.Headers.TryGetValue(StripeSignatureHeaderName, out var signature)
			? signature[0]
			: null;

		if (string.IsNullOrEmpty(stripeSignature))
		{
			throw new ServiceArgumentException($"No {StripeSignatureHeaderName} present on request");
		}

		var json = await new StreamReader(request.Body).ReadToEndAsync(cancellationToken);

		try
		{
			var stripeEvent = EventUtility.ConstructEvent(json, stripeSignature, _configuration.WebhookSecretKey);

			if (stripeEvent.Type == Events.CheckoutSessionCompleted)
			{
				var session = (Session)stripeEvent.Data.Object;

				if (!Guid.TryParse(session.ClientReferenceId, out var tourId))
				{
					throw new ServiceArgumentException($"Tour id is not of Guid type: {session.ClientReferenceId}");
				}

				var tour = await _dbContext.Tours
					.FirstOrDefaultAsync(x => x.Id == tourId, cancellationToken);

				if (tour is null)
				{
					throw new UnexpectedServiceResponseException($"Stripe Session is attached to unknown tour id: {tourId}");
				}

				var user = await _dbContext.Users
					.FirstOrDefaultAsync(x => x.Email == session.CustomerEmail, cancellationToken);

				if (user is null)
				{
					throw new UnexpectedServiceResponseException($"Stripe Session is attached to unknown user: {session.CustomerEmail}");
				}

				var price = session.AmountTotal / 100;
				if (!price.HasValue)
				{
					throw new UnexpectedServiceResponseException($"No price is attached to Stripe Session");
				}

				var newBooking = new Booking
				{
					Tour = tour,
					User = user,
					Price = price.Value,
					CreatedAt = DateTimeOffset.UtcNow,
				};

				_dbContext.Bookings.Add(newBooking);
				await _dbContext.SaveChangesAsync(cancellationToken);
			}
		}
		catch (Exception ex) when (ex is JsonException or StripeException)
		{
			throw new ServiceArgumentException("Could not parse body as a stripe event");
		}
	}
}
