using Detours.Data.Entities;

namespace Detours.StaticDeployment;

public class LocationInputBase
{
	public string Description { get; init; } = default!;

	public float X { get; init; }

	public float Y { get; init; }
}

public class StartLocationInput : LocationInputBase
{
	public string Address { get; init; } = default!;
}

public class LocationInput : LocationInputBase
{
	public byte Day { get; init; }
}

public class TourInput
{
	public StartLocationInput StartLocation { get; init; } = default!;

	public ICollection<string> Images { get; init; } = default!;

	public ICollection<DateTimeOffset> StartDates { get; init; } = default!;

	public Guid Id { get; init; }

	public string Name { get; init; } = default!;

	public byte Duration { get; init; }

	public byte MaxGroupSize { get; init; }

	public Difficulty Difficulty { get; init; }

	public ICollection<Guid> Guides { get; init; } = default!;

	public decimal Price { get; init; }

	public string Summary { get; init; } = default!;

	public string Description { get; init; } = default!;

	public string ImageCover { get; init; } = default!;

	public ICollection<LocationInput> Locations { get; init; } = default!;
}
