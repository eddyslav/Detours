using Microsoft.AspNetCore.Mvc;

using Detours.Data.Models.Responses;

using Detours.Services;

namespace Detours.Controllers;

[Route("api/tours")]
public class ToursController : BaseController
{
	private readonly ITourService _service;

	public ToursController(ITourService service)
	{
		ArgumentNullException.ThrowIfNull(service);

		_service = service;
	}

	[HttpGet]
	public async Task<ICollection<QueryTourResponse>> QueryToursAsync([FromQuery] int? page
		, CancellationToken cancellationToken) => await _service.QueryToursAsync(page, cancellationToken);

	[HttpGet("{slug}")]
	public async Task<ActionResult<TourResponse>> GetTourBySlugAsync([FromRoute] string slug
		, CancellationToken cancellationToken) => OkIfFound(await _service.FindTourBySlugAsync(slug, cancellationToken));
}
