namespace Detours.Data.Options;

public class MailSenderConfigurationBase
{

	public string SenderAddress { get; init; } = default!;

	public string SenderDisplayName { get; init; } = default!;
}
