using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Http;

using Detours.Data.Entities;

namespace Detours.Data.Models.Requests;

public class CreateTourRequest : IValidatableObject
{
	[Required(AllowEmptyStrings = false)]
	[MaxLength(40)]
	public string Name { get; init; } = default!;

	[Range(1, int.MaxValue)]
	public int Duration { get; init; }

	[Range(1, int.MaxValue)]
	public int MaxGroupSize { get; init; }

	[Range(0, double.MaxValue)]
	public decimal Price { get; init; }

	[Required(AllowEmptyStrings = false)]
	public string Summary { get; init; } = default!;

	[Required(AllowEmptyStrings = false)]
	public string Description { get; init; } = default!;

	[Required]
	[MinLength(1)]
	public ICollection<DateTimeOffset> StartDates { get; init; } = default!;

	[Required]
	public CreateTourStartLocationRequest StartLocation { get; init; } = default!;

	[Required]
	[MinLength(1)]
	public ICollection<CreateTourLocationByDayRequest> Locations { get; init; } = default!;

	[Required]
	[MinLength(1)]
	public ICollection<Guid> GuideIds { get; init; } = default!;

	[Required]
	public IFormFile ImageCover { get; init; } = default!;

	[Required]
	[MinLength(1)]
	public ICollection<IFormFile> Images { get; init; } = default!;

	public Difficulty Difficulty { get; init; }

	public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
	{
		if (!Enum.IsDefined(typeof(Difficulty), Difficulty))
		{
			yield return new ValidationResult("Invalid difficulty provided", new[] { nameof(Difficulty) });
		}
	}
}
