namespace Detours.Data.Entities;

public class TourGuide : Entity
{
	public Guid TourId { get; init; }
	public Tour Tour { get; init; } = default!;

	public Guid GuideId { get; init; }
	public User Guide { get; init; } = default!;
}
