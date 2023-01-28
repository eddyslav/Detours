using Detours.Data.Entities;

namespace Detours.Data.Models.Responses;

public class TourResponse
{
	public required Guid Id { get; init; }

	public required string Name { get; init; }

	public required int Duration { get; init; }

	public required int MaxGroupSize { get; init; }

	public required int RatingsCount { get; init; }

	public required double RatingsAverage { get; init; }

	public required Difficulty Difficulty { get; init; }

	public required decimal Price { get; init; }

	public required string Summary { get; init; }

	public required string Description { get; init; }

	public required string ImageCover { get; init; }

	public required ICollection<string> Images { get; init; }

	public required DateTimeOffset StartDate { get; init; }

	public required StartTourLocationResponse StartLocation { get; init; }

	public required ICollection<TourLocationByDayResponse> Locations { get; init; }

	public required ICollection<GuideResponse> Guides { get; init; }
}
