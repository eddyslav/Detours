using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;

namespace Detours.Authorization;

internal sealed class CustomAuthorizationPolicyResolver : DefaultAuthorizationPolicyProvider
{
	public CustomAuthorizationPolicyResolver(IOptions<AuthorizationOptions> authorizationOptions)
		: base(authorizationOptions)
	{

	}

	public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
	{
		var policy = await base.GetPolicyAsync(policyName);
		return policy ?? CustomRequirementPolicyResolver.GetPolicy(policyName);
	}
}
