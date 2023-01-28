namespace Detours.Data.Models.Responses;

public class ErrorResponse
{
	public required string Status { get; init; }
	public required ICollection<string> Messages { get; init; }
}
