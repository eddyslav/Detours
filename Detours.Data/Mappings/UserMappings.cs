using Detours.Data.Entities;
using Detours.Data.Models.Responses;

namespace Detours.Data.Mappings;

public static class UserMappings
{
	public static UserResponse ToResponse(this User user)
	{
		ArgumentNullException.ThrowIfNull(user);

		return new UserResponse
		{
			Id = user.Id,
			Name = user.Name,
			Email = user.Email,
			Photo = user.Photo,
			Role = user.Role,
		};
	}
}
