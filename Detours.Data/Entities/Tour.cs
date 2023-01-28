namespace Detours.Data.Entities;

public enum Difficulty
{
	Easy,
	Medium,
	Difficult
}

public class Tour : Entity
{
	public string Name { get; init; } = default!;
	
	public string Slug { get; init; } = default!;

	public int Duration { get; init; }
	
	public int MaxGroupSize { get; init; }
	
	public Difficulty Difficulty { get; init; }
	
	public decimal Price { get; init; }

	public string Summary { get; init; } = default!;

	public string Description { get; init; } = default!;

	public TourImageCover ImageCover { get; set; } = default!;

	public ICollection<TourImage> Images { get; set; } = default!;

	public ICollection<TourGuide> Guides { get; init; } = default!;

	public ICollection<TourStartDate> StartDates { get; init; } = default!;

	public StartTourLocation StartLocation { get; init; } = default!;

	public ICollection<TourLocationByDay> Locations { get; init; } = default!;

	public ICollection<Review> Reviews { get; init; } = default!;
}
