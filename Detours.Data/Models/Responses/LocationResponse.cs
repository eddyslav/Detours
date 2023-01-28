namespace Detours.Data.Models.Responses;

public class LocationResponse
{
	public float X { get; init; }

	public float Y { get; init; }

	public string Description { get; init; } = default!;

	public string Address { get; init; } = default!;
}
