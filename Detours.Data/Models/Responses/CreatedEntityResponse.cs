using Detours.Data.Entities;

namespace Detours.Data.Models.Responses;

public class CreatedEntityResponse
{
	public Guid Id { get; }

	public CreatedEntityResponse(Guid id)
	{
		Id = id;
	}

	public CreatedEntityResponse(Entity entity)
		: this(entity.Id)
	{
	}
}
