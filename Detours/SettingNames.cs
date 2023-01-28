namespace Detours;

internal static class SettingNames
{
	public static class ConnectionStrings
	{
		public const string DetoursDb = "DetoursDb";
	}

	public static class Services
	{
		private const string Name = "Services";

		public const string Stripe = $"{Name}:Stripe";

		public const string SmtpMailSender = $"{Name}:SmtpMailSender";

		public const string SendGridMailSender = $"{Name}:SendGridMailSender";
	}

	public const string Authentication = "Authentication";
}
