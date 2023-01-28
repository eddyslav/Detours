using Microsoft.EntityFrameworkCore;

using Detours.Core;

using Detours.Data;
using Detours.Data.Entities;
using Detours.Data.Mappings;
using Detours.Data.Models.Requests;
using Detours.Data.Models.Responses;

namespace Detours.Services;

file static class Mapping
{
	public static IQueryable<ReviewResponse> ToResponse(this IQueryable<Review> reviews) =>
		reviews.Select(x => new ReviewResponse
		{
			Id = x.Id,
			Rating = x.Rating,
			User = x.User.ToResponse(),
			Description = x.Description,
		});
}

public class ReviewService : IReviewService
{	
	private readonly DetoursDbContext _dbContext;
	private readonly IUserService _userService;

	public ReviewService(DetoursDbContext dbContext
		, IUserService userService)
	{
		ArgumentNullException.ThrowIfNull(dbContext);
		ArgumentNullException.ThrowIfNull(userService);

		_dbContext = dbContext;
		_userService = userService;
	}

	public async Task<ReviewResponse?> FindReviewByIdAsync(Guid reviewId, CancellationToken cancellationToken)
	{
		var review = await _dbContext.Reviews
			.AsNoTracking()
			.Include(x => x.User)
			.Where(x => x.Id == reviewId)
			.ToResponse()
			.FirstOrDefaultAsync(cancellationToken);

		return review;
	}

	public async Task<ICollection<ReviewResponse>> GetReviewsByTourIdAsync(Guid tourId, int? page, CancellationToken cancellationToken)
	{
		var query = _dbContext.Reviews
			.Include(x => x.User)
			.Where(x => x.TourId == tourId)
			.OrderBy(x => x.CreatedAt)
			.ToResponse();

		return await query.ToListAsync(cancellationToken);
	}

	public async Task<CreatedEntityResponse> CreateNewReviewAsync(CreateReviewRequest request, CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(request);

		var tour = await _dbContext.Tours
			.FirstOrDefaultAsync(x => x.Id == request.TourId, cancellationToken);

		if (tour is null)
		{
			throw new ServiceArgumentException($"Tour with id {request.TourId} does not exist");
		}

		var currentUser = await _userService.GetCurrentUserAsync(cancellationToken);
		var newReview = new Review
		{
			Rating = request.Rating,
			Description = request.Description,
			Tour = tour,
			User = currentUser,
			CreatedAt = DateTimeOffset.UtcNow,
		};

		_dbContext.Reviews.Add(newReview);
		await _dbContext.SaveChangesAsync(cancellationToken);

		return new CreatedEntityResponse(newReview);
	}

	public async Task DeleteReviewByIdAsync(Guid reviewId, CancellationToken cancellationToken)
	{
		var review = await _dbContext.Reviews
			.FirstOrDefaultAsync(x => x.Id == reviewId, cancellationToken);

		if (review is null)
		{
			throw new ServiceArgumentException($"Review with id {reviewId} does not exist");
		}

		_dbContext.Reviews.Remove(review);
		await _dbContext.SaveChangesAsync(cancellationToken);
	}
}
