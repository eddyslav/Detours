namespace Detours.Data.Entities;

public class RefreshToken : Entity
{
	public Guid JwtId { get; init; }

	public DateTimeOffset CreatedAt { get; init; }
	public DateTimeOffset ExpiresAt { get; init; }

	public bool Used { get; set; }
	public bool Invalidated { get; set; }

	public Guid UserId { get; init; }
	public User User { get; init; } = default!;
}
