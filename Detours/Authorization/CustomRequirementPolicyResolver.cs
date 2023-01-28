using Microsoft.AspNetCore.Authorization;

namespace Detours.Authorization;

static class CustomRequirementPolicyResolver
{
	private delegate bool PolicyResolver(string source, out AuthorizationPolicy? policy);

	private static readonly IReadOnlyCollection<PolicyResolver> PolicyResolvers;

	static CustomRequirementPolicyResolver()
	{
		PolicyResolvers = new PolicyResolver[]
		{
			RequiresUserRolesAttribute.TryLoadPolicy,
		};
	}

	public static AuthorizationPolicy? GetPolicy(string serializedRequirement)
	{
		foreach (var policyResolver in PolicyResolvers)
		{
			if (policyResolver(serializedRequirement, out var policy))
			{
				return policy;
			}
		}

		return null;
	}
}
