using Microsoft.AspNetCore.Mvc;

using Detours.Authorization;

using Detours.Data.Entities;
using Detours.Data.Models.Requests;
using Detours.Data.Models.Responses;

using Detours.Services;

namespace Detours.Controllers;

[Route("api/tours")]
[RequiresUserRoles(UserRole.Admin)]
public class InternalToursController : BaseController
{
	private readonly ITourService _service;

	public InternalToursController(ITourService service)
	{
		ArgumentNullException.ThrowIfNull(service);

		_service = service;
	}

	[HttpDelete("{tourId:guid}")]
	public async Task DeleteTourByIdAsync([FromRoute] Guid tourId
		, CancellationToken cancellationToken)
	{
		await _service.DeleteTourByIdAsync(tourId, cancellationToken);
	}

	[HttpPost]
	public async Task<ActionResult<CreatedEntityResponse>> CreateToursAsync([FromForm] CreateTourRequest request
		, CancellationToken cancellationToken) => Created(await _service.CreateTourAsync(request, cancellationToken));
}
