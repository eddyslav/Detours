using Microsoft.AspNetCore.Mvc;

using Detours.Data.Models.Responses;

namespace Detours.Controllers;

[ApiController]
public class BaseController : ControllerBase
{
	protected ActionResult<TResult> OkIfFound<TResult>(TResult? value)
		=> value is null ? NotFound() : value;

	protected ActionResult<CreatedEntityResponse> Created(CreatedEntityResponse value)
		=> StatusCode(StatusCodes.Status201Created, value);
}
