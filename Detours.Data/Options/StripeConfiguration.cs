namespace Detours.Data.Options;

public class StripeConfiguration
{
	public string SecretKey { get; init; } = default!;

	public string WebhookSecretKey { get; init; } = default!;
}
