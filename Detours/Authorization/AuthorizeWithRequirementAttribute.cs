using System.Text.Json;
using System.Diagnostics.CodeAnalysis;

using Microsoft.AspNetCore.Authorization;

namespace Detours.Authorization;

internal abstract class AuthorizeWithRequirementAttribute : AuthorizeAttribute
{
	private static string FormatSerializedRequirementPrefix(string policyName)
	{
		return policyName + "|";
	}

	protected static bool TryDeserializeRequirement<TRequirement>(
		string source,
		string policyName,
		[NotNullWhen(true)] out TRequirement? requirement)
		where TRequirement : IAuthorizationRequirement
	{
		requirement = default;

		if (string.IsNullOrWhiteSpace(source))
		{
			return false;
		}

		var prefix = FormatSerializedRequirementPrefix(policyName);
		if (!source.StartsWith(prefix))
		{
			return false;
		}

		try
		{
			requirement = JsonSerializer.Deserialize<TRequirement>(source[prefix.Length..])!;
			return true;
		}
		catch (JsonException)
		{
			return false;
		}
	}

	protected static string SerializeRequirement<TRequirement>(string policyName, TRequirement requirement)
		where TRequirement : IAuthorizationRequirement
	{
		var prefix = FormatSerializedRequirementPrefix(policyName);

		return prefix + JsonSerializer.Serialize(requirement);
	}

	protected AuthorizeWithRequirementAttribute(string serializedRequirement)
		: base(serializedRequirement)
	{

	}
}
