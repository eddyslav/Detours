namespace Detours.Data.Options;

public class AuthenticationConfiguration
{
	public string Issuer { get; init; } = default!;
	public string SecretKey { get; init; } = default!;

	public TimeSpan TokenLifetime { get; init; } = TimeSpan.FromMinutes(15);
	public TimeSpan RefreshTokenLifetime { get; init; } = TimeSpan.FromHours(1);
}
