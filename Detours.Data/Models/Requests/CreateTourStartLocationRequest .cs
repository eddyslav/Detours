using System.ComponentModel.DataAnnotations;

namespace Detours.Data.Models.Requests;

public class CreateTourStartLocationRequest
{
	[Range(-90, 90)]
	public float X { get; init; }

	[Range(-180, 180)]
	public float Y { get; init; }

	public string Description { get; init; } = default!;

	public string Address { get; init; } = default!;
}
