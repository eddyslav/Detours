using Serilog;

namespace Detours.Extensions;

internal static class LoggingExtensions
{
	public static void AddDetoursLogging(this WebApplicationBuilder builder)
	{
		Log.Logger = new LoggerConfiguration()
			.ReadFrom.Configuration(builder.Configuration)
			.CreateLogger();

		builder.Logging.ClearProviders();
		builder.Logging.AddSerilog(Log.Logger);
		builder.Services.AddSingleton(Log.Logger);

		builder.Host.UseSerilog();
	}
}
