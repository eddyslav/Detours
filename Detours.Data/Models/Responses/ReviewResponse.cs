namespace Detours.Data.Models.Responses;

public class ReviewResponse
{
	public required Guid Id { get; init; }

	public required UserResponse User { get; init; }

	public required byte Rating { get; init; }
	
	public required string Description { get; init; }
}
