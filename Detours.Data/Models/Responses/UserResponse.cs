using Detours.Data.Entities;

namespace Detours.Data.Models.Responses;

public class UserResponse
{
	public Guid Id { get; init; }

	public string Email { get; init; } = default!;

	public string Name { get; init; } = default!;

	public string? Photo { get; init; }

	public UserRole Role { get; init; }
}
