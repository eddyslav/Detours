using System.Text.Json;
using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

using Detours.Core;
using Detours.Utils;

using Detours.Data.Models.Responses;

namespace Detours.Extensions;

internal static class ControllersExtensions
{
	public static IServiceCollection AddDetoursControllers(
		this IServiceCollection services)
	{
		services.AddControllers()
			.AddJsonOptions(options =>
			{
				var jsonOptions = options.JsonSerializerOptions;

				jsonOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
				jsonOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
				jsonOptions.Converters.Add(new JsDateTimeOffsetConverter());
			});

		services.Configure<ApiBehaviorOptions>(options =>
		{
			options.InvalidModelStateResponseFactory = (actionContext) =>
			{
				var errorResponse = new ErrorResponse
				{
					Status = ErrorCode.InvalidValue.StatusName,
					Messages = actionContext.ModelState
						.SelectMany(x => x.Value?.Errors ?? Enumerable.Empty<ModelError>())
						.Select(x => x.ErrorMessage)
						.ToList(),
				};

				return new BadRequestObjectResult(errorResponse);
			};
		});

		return services;
	}
}
