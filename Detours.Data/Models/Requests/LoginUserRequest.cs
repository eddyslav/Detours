using System.ComponentModel.DataAnnotations;

namespace Detours.Data.Models.Requests;

public class LoginUserRequest
{
	[EmailAddress(ErrorMessage = "Please provide a valid email address")]
	public string Email { get; init; } = default!;

	[DataType(DataType.Password)]
	public string Password { get; init; } = default!;
}
