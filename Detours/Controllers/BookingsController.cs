using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Detours.Authorization;

using Detours.Data.Entities;
using Detours.Data.Models.Responses;

using Detours.Services;

namespace Detours.Controllers;

[Route("api/bookings")]
[RequiresUserRoles(UserRole.User)]
public class BookingsController : BaseController
{
    private readonly IBookingService _service;

    public BookingsController(IBookingService service)
    {
        ArgumentNullException.ThrowIfNull(service);

        _service = service;
    }

    [HttpGet("my")]
    public async Task<ICollection<BookingResponse>> GetMyBookingsAsync([FromQuery] int? page
        , CancellationToken cancellationToken) => await _service.GetMyBookingsAsync(page, cancellationToken);

    [HttpGet("checkout-session/{tourId:guid}")]
    public async Task<SessionCheckoutResponse> GetCheckoutSessionAsync([FromRoute] Guid tourId
        , CancellationToken cancellationToken) => await _service.GetCheckoutSessionAsync(tourId, cancellationToken);

    [AllowAnonymous]
    [HttpPost("webhook-checkout")]
    public async Task ProcessCheckoutAsync(CancellationToken cancellationToken) =>
        await _service.ProcessCheckoutAsync(cancellationToken);
}
