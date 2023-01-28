namespace Detours.Data.Models.Responses;

public class TourLocationByDayResponse
{
	public byte Day { get; init; }

	public float X { get; init; }

	public float Y { get; init; }

	public string Description { get; init; } = default!;
}
