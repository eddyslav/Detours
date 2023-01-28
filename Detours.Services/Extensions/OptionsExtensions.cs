using Microsoft.Extensions.Options;

namespace Detours.Services.Extensions;

public static class OptionsExtensions
{
	public static TOptions GetConfiguration<TOptions>(this IOptions<TOptions> options)
		where TOptions : class
	{
		return options?.Value ?? throw new InvalidOperationException("No configuration provided");
	}
}
