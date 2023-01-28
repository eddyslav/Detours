namespace Detours.Data.Entities;

public class Booking : Entity
{
	public Guid UserId { get; init; }
	public User User { get; init; } = default!;

	public Guid TourId { get; init; }
	public Tour Tour { get; init; } = default!;

	public decimal Price { get; init; }

	public DateTimeOffset CreatedAt { get; init; }
}
