using Detours.Data.Models.Responses;

namespace Detours.Services;

public interface IBookingService
{
	Task<ICollection<BookingResponse>> GetMyBookingsAsync(int? page, CancellationToken cancellationToken);

	Task<SessionCheckoutResponse> GetCheckoutSessionAsync(Guid tourId, CancellationToken cancellationToken);

	Task ProcessCheckoutAsync(CancellationToken cancellationToken);
}
