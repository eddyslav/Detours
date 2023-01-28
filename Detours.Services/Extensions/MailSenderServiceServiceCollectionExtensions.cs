using System.Net;
using System.Net.Mail;

using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.DependencyInjection;

using Detours.Data.Options;

namespace Detours.Services.Extensions;

public static class MailSenderServiceServiceCollectionExtensions
{
	private static FluentEmailServicesBuilder AddMailSenderServiceBase(this IServiceCollection services, MailSenderConfigurationBase configuration)
	{
		var rootPath = Path.Combine(Environment.CurrentDirectory, "templates/emails");

		return services.AddFluentEmail(configuration.SenderAddress, configuration.SenderDisplayName)
			.AddLiquidRenderer(options => options.FileProvider = new PhysicalFileProvider(rootPath));
	}

	public static IServiceCollection AddSmtpMailSenderService(this IServiceCollection services
		, Action<SmtpMailSenderConfiguration> configure)
	{
		var configuration = new SmtpMailSenderConfiguration();
		configure(configuration);

		var smtpClient = new SmtpClient
		{
			Host = configuration.Host,
			Port = configuration.Port,
			UseDefaultCredentials = false,
			EnableSsl = true,
			Credentials = new NetworkCredential(configuration.UserName, configuration.Password),
		};

		services.AddMailSenderServiceBase(configuration)
			.AddSmtpSender(smtpClient);

		return services;
	}

	public static IServiceCollection AddSendGridMailSenderService(this IServiceCollection services
		, Action<SendGridMailSenderConfiguration> configure)
	{
		var configuration = new SendGridMailSenderConfiguration();
		configure(configuration);

		services.AddMailSenderServiceBase(configuration)
			.AddSendGridSender(configuration.ApiKey, configuration.SandBoxMode);

		return services;
	}
}
