using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Http;

namespace Detours.Data.Models.Requests;

public class RegisterUserRequest
{
	[EmailAddress(ErrorMessage = "Please provide a valid email address")]
	public string Email { get; init; } = default!;

	[RegularExpression("^[A-Z][a-z]*(\\s[A-Z][a-z]*)+$", ErrorMessage = "Please provide a valid full name")]
	public string Name { get; init; } = default!;

	[RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[#$^+=!*()@%&]).{8,}$", ErrorMessage = "Please provide a valid password")]
	public string Password { get; init; } = default!;

	public IFormFile? Photo { get; init; }

	[Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
	public string PasswordConfirm { get; init; } = default!;
}
