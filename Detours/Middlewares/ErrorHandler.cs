using System.Net.Mime;

using ILogger = Serilog.ILogger;

using Detours.Core;
using Detours.Data.Models.Responses;

namespace Detours.Middlewares;

internal sealed class ErrorHandler
{
	private readonly RequestDelegate _nextHandler;

	private static Task HandleExceptionAsync(HttpContext httpContext, Exception exception, ILogger logger)
	{
		logger.Error(exception, "Unhandled error caught");

		var errorCode = exception is CoreException errorCodeException
			? errorCodeException.ErrorCode
			: ErrorCode.InternalServerError;

		var response = httpContext.Response;
		response.ContentType = MediaTypeNames.Application.Json;
		response.StatusCode = errorCode.StatusCode;

		var errorResponse = new ErrorResponse
		{
			Status = errorCode.Name,
			Messages = new[] { exception.Message },
		};

		return response.WriteAsJsonAsync(errorResponse);
	}

	public ErrorHandler(RequestDelegate nextHandler)
	{
		_nextHandler = nextHandler;
	}

	public async Task InvokeAsync(HttpContext context, ILogger logger)
	{
		try
		{
			await _nextHandler(context);
		}
		catch (Exception ex)
		{
			await HandleExceptionAsync(context, ex, logger);
		}
	}
}
