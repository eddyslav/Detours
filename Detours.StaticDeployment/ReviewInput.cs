namespace Detours.StaticDeployment;

public class ReviewInput
{
	public byte Rating { get; init; }

	public string Description { get; init; } = default!;

	public Guid TourId { get; init; }

	public Guid UserId { get; init; }
}
