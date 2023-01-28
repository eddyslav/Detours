using System.Net.Mail;

using MediatR;

using Serilog;

using FluentEmail.Core;

namespace Detours.Services.Notifications;

public abstract class EmailNotification : INotification
{
	public required string UserName { get; init; }
	
	public required string UserEmail { get; init; }
}

public abstract class EmailNotificationHandlerBase<TNotification> : INotificationHandler<TNotification>
	where TNotification : EmailNotification
{
	protected abstract string Subject { get; }

	protected abstract string TemplateName { get; }

	protected virtual int MaxSendAttemptsCount { get; } = 3;
	protected virtual TimeSpan SendAttemptDelay { get; } = TimeSpan.FromSeconds(15);

	protected virtual string Tag { get; } = Guid.NewGuid().ToString();

	protected IFluentEmail MailSender { get; }

	protected ILogger Logger { get; }

	protected EmailNotificationHandlerBase(IFluentEmail mailSender, ILogger logger)
	{
		ArgumentNullException.ThrowIfNull(mailSender);
		ArgumentNullException.ThrowIfNull(logger);

		MailSender = mailSender;
		Logger = logger;
	}

	public virtual async Task Handle(TNotification notification, CancellationToken cancellationToken)
	{
		Logger.Debug("New notification has been received for [{UserEmail}]", notification.UserEmail);

		var attemptsLeft = MaxSendAttemptsCount;

		do
		{
			try
			{
				var response = await MailSender.To(notification.UserEmail)
					.UsingTemplateFromFile(TemplateName, notification)
					.Subject(Subject)
					.Tag(Tag)
					.SendAsync(cancellationToken);

				if (!response.Successful)
				{
					throw new AggregateException(response.ErrorMessages.Select(x => new Exception(x)));
				}

				Logger.Debug("Notification [{MessageId}] has been sent", response.MessageId);

				return;
			}
			catch (SmtpException ex) when (attemptsLeft > 0 && (ex.StatusCode is SmtpStatusCode.MailboxBusy or SmtpStatusCode.MailboxUnavailable or SmtpStatusCode.TransactionFailed))
			{
				Logger.Error(ex, "Error while sending an email");

				await Task.Delay(SendAttemptDelay, cancellationToken);
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Error while sending an email");
				return;
			}
		} while (attemptsLeft-- > 0);
	}
}
