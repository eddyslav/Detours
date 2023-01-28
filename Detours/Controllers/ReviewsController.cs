using Microsoft.AspNetCore.Mvc;

using Detours.Authorization;

using Detours.Data.Entities;
using Detours.Data.Models.Requests;
using Detours.Data.Models.Responses;

using Detours.Services;

namespace Detours.Controllers;

[ApiController]
[Route("api/reviews")]
public class ReviewsController : BaseController
{
	private readonly IReviewService _service;

	public ReviewsController(IReviewService service)
	{
		ArgumentNullException.ThrowIfNull(service);

		_service = service;
	}

	[HttpGet("{reviewId:guid}")]
	public async Task<ActionResult<ReviewResponse>> GetReviewByIdAsync([FromRoute] Guid reviewId
		, CancellationToken cancellationToken) => OkIfFound(await _service.FindReviewByIdAsync(reviewId, cancellationToken));

	[HttpGet("byTour/{tourId:guid}")]
	public async Task<ICollection<ReviewResponse>> GetReviewsByTourIdAsync([FromRoute] Guid tourId
		, [FromQuery] int? page
		, CancellationToken cancellationToken) => await _service.GetReviewsByTourIdAsync(tourId, page, cancellationToken);

	[HttpPost]
	[RequiresUserRoles(UserRole.User)]
	public async Task<ActionResult<CreatedEntityResponse>> CreateNewReviewAsync([FromBody] CreateReviewRequest request
		, CancellationToken cancellationToken) => Created(await _service.CreateNewReviewAsync(request, cancellationToken));

	[HttpDelete("{reviewId:guid}")]
	[RequiresUserRoles(UserRole.Admin)]
	public async Task DeleteReviewByIdAsync([FromRoute] Guid reviewId
		, CancellationToken cancellationToken)
	{
		await _service.DeleteReviewByIdAsync(reviewId, cancellationToken);
	}
}
