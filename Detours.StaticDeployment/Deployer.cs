using System.Text.Json;

using Microsoft.EntityFrameworkCore;

using Detours.Core.Utils;

using Detours.Data;
using Detours.Data.Entities;
using System.Text.Json.Serialization;

namespace Detours.StaticDeployment;

public class Deployer : IDisposable
{
	private string? _usersPath;
	private string? _toursPath;
	private string? _reviewsPath;

	private DetoursDbContext _dbContext = default!;

	private static readonly JsonSerializerOptions jsonSerializerOptions;

	private bool IsAny => _usersPath is not null
		|| _toursPath is not null
		|| _reviewsPath is not null;

	static Deployer()
	{
		jsonSerializerOptions = new JsonSerializerOptions
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		};

		jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
	}

	public Deployer()
	{

	}

	public Deployer WithUsers(string usersPath)
	{
		if (File.Exists(usersPath))
		{
			_usersPath = usersPath;
		}

		return this;
	}

	public Deployer WithTours(string toursPath)
	{
		if (File.Exists(toursPath))
		{
			_toursPath = toursPath;
		}

		return this;
	}

	public Deployer WithReviews(string reviewsPath)
	{
		if (File.Exists(reviewsPath))
		{
			_reviewsPath = reviewsPath;
		}

		return this;
	}

	public Deployer WithConnectionString(string connectionString)
	{
		if (string.IsNullOrEmpty(connectionString))
		{
			return this;
		}

		var options = new DbContextOptionsBuilder<DetoursDbContext>()
			.UseSqlServer(connectionString)
			.Options;

		_dbContext = new DetoursDbContext(options);

		return this;
	}

	public async Task RunAsync(CancellationToken cancellationToken)
	{
		if (!IsAny)
		{
			return;
		}

		if (_dbContext is null)
		{
			throw new InvalidOperationException("Cannot proceed without db context");
		}

		await LoadUsersAsync(cancellationToken);
		await LoadToursAsync(cancellationToken);
		await LoadReviewsAsync(cancellationToken);
	}

	public void Dispose()
	{
		_dbContext?.Dispose();

		GC.SuppressFinalize(this);
	}

	private async Task LoadUsersAsync(CancellationToken cancellationToken)
	{
		static User Mapper(UserInput inp)
		{
			return new User
			{
				Id = inp.Id,
				Email = inp.Email,
				Name = inp.Name,
				Password = inp.Password,
				Photo = inp.Photo,
				Role = inp.Role,
			};
		}

		await LoadAsync<UserInput, User>(_usersPath, Mapper, cancellationToken);
	}

	private async Task LoadToursAsync(CancellationToken cancellationToken)
	{
		static Tour Mapper(TourInput inp)
		{
			return new Tour
			{
				Id = inp.Id,
				Name = inp.Name,
				Slug = Slugifier.Slugify(inp.Name),
				Duration = inp.Duration,
				MaxGroupSize = inp.MaxGroupSize,
				Difficulty = inp.Difficulty,
				Price = inp.Price,
				Summary = inp.Summary,
				Description = inp.Description,
				ImageCover = new TourImageCover
				{
					Image = inp.ImageCover,
				},
				Guides = inp.Guides.Select(x => new TourGuide
				{
					GuideId = x,
				}).ToList(),
				StartDates = inp.StartDates.Select(x => new TourStartDate
				{
					Date = x,
				}).ToList(),
				StartLocation = new StartTourLocation
				{
					Address = inp.StartLocation.Address,
					Description = inp.StartLocation.Description,
					X = inp.StartLocation.X,
					Y = inp.StartLocation.Y,
				},
				Locations = inp.Locations.Select(x => new TourLocationByDay
				{
					Day = x.Day,
					Description = x.Description,
					X = x.X,
					Y = x.Y,
				}).ToList(),
				Images = inp.Images.Select(x => new TourImage
				{
					Image = x,
				}).ToList(),
			};
		}

		await LoadAsync<TourInput, Tour>(_toursPath, Mapper, cancellationToken);
	}

	private async Task LoadReviewsAsync(CancellationToken cancellationToken)
	{
		static Review Mapper(ReviewInput inp)
		{
			return new Review
			{
				Rating = inp.Rating,
				Description = inp.Description,
				TourId = inp.TourId,
				UserId = inp.UserId,
			};
		}

		await LoadAsync<ReviewInput, Review>(_reviewsPath, Mapper, cancellationToken);
	}

	private async Task LoadAsync<TInput, TEntity>(string? targetPath
		, Func<TInput, TEntity> mapper
		, CancellationToken cancellationToken)
		where TInput : class
		where TEntity : class
	{
		if (string.IsNullOrEmpty(targetPath))
		{
			return;
		}

		var inputs = ReadJson<TInput>(targetPath);
		var entities = inputs.Select(mapper);

		_dbContext.Set<TEntity>().AddRange(entities);
		await _dbContext.SaveChangesAsync(cancellationToken);
	}

	private static ICollection<TResult> ReadJson<TResult>(string path)
		where TResult : class
	{
		var json = File.ReadAllText(path);
		return JsonSerializer.Deserialize<ICollection<TResult>>(json, jsonSerializerOptions)
			?? throw new InvalidOperationException();
	}
}
