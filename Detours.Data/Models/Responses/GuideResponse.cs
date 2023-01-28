using Detours.Data.Entities;

namespace Detours.Data.Models.Responses;

public class GuideResponse
{
	public required string Name { get; init; }

	public required string? Photo { get; init; }

	public required UserRole Role { get; init; }
}
