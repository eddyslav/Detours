using Detours.Data.Models.Requests;
using Detours.Data.Models.Responses;

namespace Detours.Services;

public interface IReviewService
{
	Task<ReviewResponse?> FindReviewByIdAsync(Guid reviewId, CancellationToken cancellationToken);

	Task<ICollection<ReviewResponse>> GetReviewsByTourIdAsync(Guid tourId, int? page, CancellationToken cancellationToken);

	Task<CreatedEntityResponse> CreateNewReviewAsync(CreateReviewRequest request, CancellationToken cancellationToken);

	Task DeleteReviewByIdAsync(Guid reviewId, CancellationToken cancellationToken);
}
