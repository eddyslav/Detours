namespace Detours.Data.Entities;

public class TourLocationByDay : TourLocation
{
	public Guid TourId { get; init; }
	public Tour Tour { get; init; } = default!;

	public byte Day { get; init; }
}
