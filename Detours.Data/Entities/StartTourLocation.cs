namespace Detours.Data.Entities;

public class StartTourLocation : TourLocation
{
	public Guid TourId { get; init; }
	public Tour Tour { get; init; } = default!;

	public string Address { get; init; } = default!;
}
