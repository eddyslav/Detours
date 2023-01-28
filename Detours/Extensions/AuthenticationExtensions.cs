using System.Text;

using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using Detours.Data.Options;

namespace Detours.Extensions;

internal static class AuthenticationExtensions
{
	public static IServiceCollection AddDetoursAuthentication(this IServiceCollection services
		, IConfiguration configuration)
	{
		var authenticationSection = configuration.GetRequiredSection(SettingNames.Authentication);
		services
			.AddOptions<AuthenticationConfiguration>()
			.Configure(authenticationSection.Bind)
			.PostConfigure(options =>
			{
				if (string.IsNullOrWhiteSpace(options.Issuer))
				{
					throw new Exception("Issuer cannot be null or empty");
				}

				if (string.IsNullOrWhiteSpace(options.SecretKey))
				{
					throw new Exception("Secret key cannot be null or empty");
				}
			});

		var authenticationConfiguration = new AuthenticationConfiguration();
		authenticationSection.Bind(authenticationConfiguration);

		services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			.AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,

					ValidIssuer = authenticationConfiguration.Issuer,
					ValidAudience = authenticationConfiguration.Issuer,

					IssuerSigningKey =
						new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationConfiguration.SecretKey)),
				};
			});

		return services;
	}
}
