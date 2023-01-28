using Microsoft.AspNetCore.Authorization;

using Detours.Data.Entities;

namespace Detours.Authorization;

internal class RequiresUserRolesAttribute : AuthorizeWithRequirementAttribute
{
	private const string PolicyName = nameof(UserRolesRequirement);

	private static UserRolesRequirement CreateRequirement(UserRole userRoles)
	{
		return new UserRolesRequirement(userRoles);
	}

	public static bool TryLoadPolicy(string source, out AuthorizationPolicy? policy)
	{
		if (TryDeserializeRequirement<UserRolesRequirement>(source, PolicyName, out var requirement))
		{
			policy = new AuthorizationPolicyBuilder()
				.AddRequirements(requirement)
				.Build();

			return true;
		}

		policy = null;
		return false;
	}

	public RequiresUserRolesAttribute(UserRole userRoles)
		: base(SerializeRequirement(PolicyName, CreateRequirement(userRoles)))
	{
	}
}
