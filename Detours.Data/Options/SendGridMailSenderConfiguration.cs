namespace Detours.Data.Options;

public class SendGridMailSenderConfiguration : MailSenderConfigurationBase
{
	public string ApiKey { get; init; } = default!;

	public bool SandBoxMode { get; init; } = false;
}
