using Serilog;
using Serilog.Configuration;

namespace Detours.Logging.Extensions;

public static class LoggingExtensions
{
	public static LoggerConfiguration WithOperationId(this LoggerEnrichmentConfiguration enrichConfiguration)
	{
		return enrichConfiguration.With<OperationIdEnricher>();
	}
}
