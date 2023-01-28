using Detours.Data.Models.Requests;
using Detours.Data.Models.Responses;

namespace Detours.Services;

public interface ITourService
{
	Task<ICollection<QueryTourResponse>> QueryToursAsync(int? page, CancellationToken cancellationToken);

	Task<TourResponse?> FindTourBySlugAsync(string slug, CancellationToken cancellationToken);

	Task<CreatedEntityResponse> CreateTourAsync(CreateTourRequest request, CancellationToken cancellationToken);

	Task DeleteTourByIdAsync(Guid tourId, CancellationToken cancellationToken);
}
