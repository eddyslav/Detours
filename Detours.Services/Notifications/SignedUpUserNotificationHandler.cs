using Serilog;

using FluentEmail.Core;

namespace Detours.Services.Notifications;

public class SignedUpUserNotification : EmailNotification
{
}

public class SignedUpUserNotificationHandler : EmailNotificationHandlerBase<SignedUpUserNotification>
{
	protected override string Subject => "Welcome to Detours family!";

	protected override string TemplateName => "templates/emails/welcomeNotification.liquid";

	protected override string Tag => "welcome_email";

	public SignedUpUserNotificationHandler(IFluentEmail mailSender, ILogger logger)
		: base(mailSender, logger)
	{

	}
}
