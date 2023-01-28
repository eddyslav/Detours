using System.ComponentModel.DataAnnotations;

namespace Detours.Data.Models.Requests;

public class CreateReviewRequest
{
	public Guid TourId { get; init; }

	[Range(1, 5)]
	public byte Rating { get; init; }

	[Required(AllowEmptyStrings = false)]
	[MaxLength(255)]
	public string Description { get; init; } = default!;
}
