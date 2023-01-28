using Microsoft.Extensions.DependencyInjection;

using Stripe.Checkout;
using StripeConfigurationGlobal = Stripe.StripeConfiguration;

using Detours.Data.Options;

namespace Detours.Services.Extensions;

public static class BookingServiceServiceCollectionExtensions
{
	public static IServiceCollection AddBookingService(this IServiceCollection services
		, Action<StripeConfiguration> configureOptions)
	{
		services.AddOptions<StripeConfiguration>()
			.Configure(configureOptions)
			.PostConfigure(x =>
			{
				if (string.IsNullOrWhiteSpace(x.SecretKey))
				{
					throw new Exception($"\"{nameof(x.SecretKey)}\" cannot be null or empty");
				}

				if (string.IsNullOrWhiteSpace(x.WebhookSecretKey))
				{
					throw new Exception($"\"{nameof(x.WebhookSecretKey)}\" cannot be null or empty");
				}

				StripeConfigurationGlobal.ApiKey = x.SecretKey;
			});

		services.AddScoped<SessionService>();
		services.AddScoped<IBookingService, BookingService>();

		return services;
	}
}
