namespace Detours.Data.Models.Responses;

public class StartTourLocationResponse
{
	public required float X { get; init; }

	public required float Y { get; init; }

	public required string Address { get; init; }

	public required string Description { get; init; }
}
