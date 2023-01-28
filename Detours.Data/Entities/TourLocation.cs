namespace Detours.Data.Entities;

public abstract class TourLocation : Entity
{
	public float X { get; init; }

	public float Y { get; init; }

	public string Description { get; init; } = default!;
}
