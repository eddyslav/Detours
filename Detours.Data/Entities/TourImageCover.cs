namespace Detours.Data.Entities;

public class TourImageCover : Entity
{
	public Guid TourId { get; init; }
	public Tour Tour { get; init; } = default!;

	public string Image { get; init; } = default!;
}
