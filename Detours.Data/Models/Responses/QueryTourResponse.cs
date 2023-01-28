using Detours.Data.Entities;

namespace Detours.Data.Models.Responses;

public class QueryTourResponse
{
	public required string Name { get; init; }

	public required string Slug { get; init; }

	public required string Summary { get; init; }

	public required string ImageCover { get; init; }

	public required string StartLocation { get; init; }

	public required Difficulty Difficulty { get; init; }

	public required DateTimeOffset StartDate { get; init; }

	public required int LocationsCount { get; init; }

	public required int MaxGroupSize { get; init; }

	public required decimal Price { get; init; }

	public required int RatingsCount { get; init; }

	public required double RatingsAverage { get; init; }
}
