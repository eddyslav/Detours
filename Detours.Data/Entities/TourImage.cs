namespace Detours.Data.Entities;

public class TourImage : Entity
{
	public Guid TourId { get; init; }
	public Tour Tour { get; init; } = default!;

	public string Image { get; init; } = default!;
}
