namespace Detours.Data.Entities;

public class TourStartDate : Entity
{
	public Guid TourId { get; init; }
	public Tour Tour { get; init; } = default!;

	public DateTimeOffset Date { get; init; }
}
