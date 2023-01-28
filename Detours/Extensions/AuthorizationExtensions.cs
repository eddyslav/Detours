using Detours.Authorization;

using Microsoft.AspNetCore.Authorization;

namespace Detours.Extensions;

internal static class AuthorizationExtensions
{
	public static IServiceCollection AddDetoursAuthorization(this IServiceCollection services)
	{
		services.AddScoped<IAuthorizationHandler, UserRolesAuthorizationHandler>();

		services.AddSingleton<IAuthorizationPolicyProvider, CustomAuthorizationPolicyResolver>();

		return services;
	}
}
