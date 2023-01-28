using System.ComponentModel.DataAnnotations.Schema;

namespace Detours.Data.Entities;

public abstract class Entity
{
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public Guid Id { get; init; }
}
