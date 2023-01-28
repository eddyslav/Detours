using Detours.Data.Entities;

namespace Detours.StaticDeployment;

public class UserInput
{
	public Guid Id { get; init; }

	public string Email { get; init; } = default!;

	public string Name { get; init; } = default!;

	public UserRole Role { get; init; }

	public string Photo { get; init; } = default!;

	public string Password { get; init; } = default!;
}
