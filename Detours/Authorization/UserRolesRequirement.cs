using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Authorization;

using Detours.Data.Entities;

namespace Detours.Authorization;

internal class UserRolesRequirement : IAuthorizationRequirement
{
	public UserRole Roles { get; }

	[JsonConstructor]
	public UserRolesRequirement(UserRole roles)
	{
		Roles = roles;
	}
}
