namespace Detours.Data.Options;

public class SmtpMailSenderConfiguration : MailSenderConfigurationBase
{
	public string Host { get; init; } = default!;

	public int Port { get; init; }

	public string UserName { get; init; } = default!;

	public string Password { get; init; } = default!;
}
