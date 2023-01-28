using Microsoft.AspNetCore.Http;

using Microsoft.EntityFrameworkCore;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

using Detours.Core;
using Detours.Core.Utils;

using Detours.Data;
using Detours.Data.Entities;
using Detours.Data.Models.Requests;
using Detours.Data.Models.Responses;

namespace Detours.Services;

public class TourService : ITourService
{
	private readonly DetoursDbContext _dbContext;

	private const string FileCoverTemplate = "tour-{0}-{1}-cover.jpeg";
	private const string FileTemplate = "tour-{0}-{1}-{2}.jpeg";

	private static async Task UploadImageAsync(string fileName
		, IFormFile formFile
		, CancellationToken cancellationToken)
	{
		var img = await Image.LoadAsync(formFile.OpenReadStream(), cancellationToken);
		img.Mutate(x => x.Resize(2000, 1333));
		await img.SaveAsJpegAsync($"public/img/tours/{fileName}", cancellationToken);
	}

	public TourService(DetoursDbContext dbContext)
	{
		ArgumentNullException.ThrowIfNull(dbContext);

		_dbContext = dbContext;
	}

	public async Task<ICollection<QueryTourResponse>> QueryToursAsync(int? page, CancellationToken cancellationToken)
	{
		var query = _dbContext.Tours
			.Include(x => x.ImageCover)
			.Include(x => x.StartDates)
			.Include(x => x.Reviews)
			.Include(x => x.StartLocation)
			.Include(x => x.Locations)
			.OrderBy(x => x.Id)
			.AsNoTracking()
			.Select(x => new QueryTourResponse
			{
				Name = x.Name,
				Slug = x.Slug,
				Summary = x.Summary,
				ImageCover = x.ImageCover.Image,
				StartLocation = x.StartLocation.Description,
				Difficulty = x.Difficulty,
				StartDate = x.StartDates.OrderBy(x => x.Date).First().Date,
				LocationsCount = x.Locations.Count,
				MaxGroupSize = x.MaxGroupSize,
				Price = x.Price,
				RatingsCount = x.Reviews.Count,
				RatingsAverage = x.Reviews.Average(x => x.Rating),
			});

		return await query.ToListAsync(cancellationToken);
	}

	public async Task<TourResponse?> FindTourBySlugAsync(string slug, CancellationToken cancellationToken)
	{
		if (string.IsNullOrWhiteSpace(slug))
		{
			throw new ArgumentException($"\"{nameof(slug)}\" cannot be null or empty", nameof(slug));
		}

		return await _dbContext.Tours
			.Include(x => x.ImageCover)
			.Include(x => x.Images)
			.Include(x => x.Guides)
				.ThenInclude(x => x.Guide)
			.Include(x => x.StartDates)
			.Include(x => x.StartLocation)
			.Include(x => x.Locations)
			.Include(x => x.Reviews)
			.Where(x => x.Slug == slug)
			.AsSplitQuery()
			.AsNoTracking()
			.Select(x => new TourResponse
			{
				Id = x.Id,
				Name = x.Name,
				Duration = x.Duration,
				MaxGroupSize = x.MaxGroupSize,
				RatingsCount = x.Reviews.Count,
				RatingsAverage = x.Reviews.Average(x => x.Rating),
				Difficulty = x.Difficulty,
				Price = x.Price,
				Summary = x.Summary,
				Description = x.Description,
				ImageCover = x.ImageCover.Image,
				Images = x.Images.Select(x => x.Image).ToArray(),
				StartDate = x.StartDates.OrderBy(x => x.Date).First().Date,
				StartLocation = new StartTourLocationResponse
				{
					X = x.StartLocation.X,
					Y = x.StartLocation.Y,
					Address = x.StartLocation.Address,
					Description = x.StartLocation.Description,
				},
				Locations = x.Locations.Select(x => new TourLocationByDayResponse
				{
					Day = x.Day,
					X = x.X,
					Y = x.Y,
					Description = x.Description,
				}).ToArray(),
				Guides = x.Guides.Select(x => new GuideResponse
				{
					Name = x.Guide.Name,
					Photo = x.Guide.Photo,
					Role = x.Guide.Role,
				}).ToArray(),
			})
			.FirstOrDefaultAsync(cancellationToken);
	}

	public async Task<CreatedEntityResponse> CreateTourAsync(CreateTourRequest request, CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(request);

		var existing = await _dbContext.Tours
			.AnyAsync(x => x.Name == request.Name, cancellationToken);

		if (existing)
		{
			throw new ServiceArgumentException($"\"{request.Name}\" tour already exist");
		}

		var guides = await _dbContext.Users
			.Where(x => request.GuideIds.Contains(x.Id)
				&& (x.Role == UserRole.Guide || x.Role == UserRole.LeadGuide))
			.ToListAsync(cancellationToken);

		if (guides.Count != request.GuideIds.Count)
		{
			var missingGuidesIds = request.GuideIds.Except(guides.Select(x => x.Id));

			throw new ServiceArgumentException(
				$"Intended tour guides were not found: {string.Join(", ", missingGuidesIds)}");
		}

		var newTour = new Tour
		{
			Id = Guid.NewGuid(),
			Name = request.Name,
			Slug = Slugifier.Slugify(request.Name),
			Duration = request.Duration,
			MaxGroupSize = request.MaxGroupSize,
			Difficulty = request.Difficulty,
			Price = request.Price,
			Summary = request.Summary,
			Description = request.Description,
			StartLocation = new StartTourLocation
			{
				Address = request.StartLocation.Address,
				Description = request.StartLocation.Description,
				X = request.StartLocation.X,
				Y = request.StartLocation.Y,
			},
			Locations = request.Locations.Select(x => new TourLocationByDay
			{
				Day = x.Day,
				X = x.X,
				Y = x.Y,
				Description = x.Description,
			}).ToList(),
			StartDates = request.StartDates.Select(x => new TourStartDate
			{
				Date = x,
			}).ToList(),
			Guides = guides.Select(x => new TourGuide
			{
				Guide = x,
			}).ToList(),
		};

		var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

		var coverFileName = string.Format(FileCoverTemplate, newTour.Id, now);
		var imagesTasks = new List<Task>(request.Images.Count + 1)
		{
			UploadImageAsync(coverFileName, request.ImageCover, cancellationToken)
		};

		var i = 1;
		var fileNames = new List<string>(request.Images.Count);
		foreach (var img in request.Images)
		{
			var fileName = string.Format(FileTemplate, newTour.Id, now, i);
			imagesTasks.Add(UploadImageAsync(fileName, img, cancellationToken));

			fileNames.Add(fileName);
			i++;
		}

		await Task.WhenAll(imagesTasks);

		newTour.ImageCover = new TourImageCover { Image = coverFileName };
		newTour.Images = fileNames.Select(x => new TourImage { Image = x }).ToList();

		_dbContext.Tours.Add(newTour);
		await _dbContext.SaveChangesAsync(cancellationToken);

		return new CreatedEntityResponse(newTour);
	}

	public async Task DeleteTourByIdAsync(Guid tourId, CancellationToken cancellationToken)
	{
		var tour = await _dbContext.Tours
			.FirstOrDefaultAsync(x => x.Id == tourId, cancellationToken);

		if (tour is null)
		{
			throw new ServiceArgumentException($"Tour with id {tourId} does not exist");
		}

		_dbContext.Tours.Remove(tour);
		await _dbContext.SaveChangesAsync(cancellationToken);
	}
}
