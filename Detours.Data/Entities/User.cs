namespace Detours.Data.Entities;

[Flags]
public enum UserRole
{
	User = 1,
	Admin = 2,
	Guide = 4,
	LeadGuide = 8,
};

public class User : Entity
{
	public string Email { get; set; } = default!;

	public string Name { get; set; } = default!;

	public string? Photo { get; set; }

	public string Password { get; set; } = default!;
	
	public UserRole Role { get; set; }
}
