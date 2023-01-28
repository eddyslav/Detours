namespace Detours.Data.Models.Responses;

public class TokenResponse
{
	public required string Token { get; init; }

	public required Guid RefreshToken { get; init; }

	public required DateTimeOffset ExpiresAt { get; init; }
}
