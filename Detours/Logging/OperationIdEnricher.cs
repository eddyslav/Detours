using System.Diagnostics;

using Serilog.Core;
using Serilog.Events;

namespace Detours.Logging;

sealed class OperationIdEnricher : ILogEventEnricher
{
	private const string OperationId = "OperationId";

	public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
	{
		var activity = Activity.Current;
		if (activity is null)
		{
			return;
		}

		var operationIdProperty = propertyFactory.CreateProperty(OperationId, activity.RootId);

		logEvent.AddPropertyIfAbsent(operationIdProperty);
	}
}
