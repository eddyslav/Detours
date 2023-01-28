namespace Detours.Data.Entities;

public class Review : Entity
{
	public Guid UserId { get; init; }
	public User User { get; init; } = default!;

	public byte Rating { get; init; } = default!;

	public string Description { get; init; } = default!;

	public Guid TourId { get; init; }
	public Tour Tour { get; init; } = default!;

	public DateTimeOffset CreatedAt { get; init; }
}
