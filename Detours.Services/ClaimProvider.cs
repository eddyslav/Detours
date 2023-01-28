using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

using Microsoft.AspNetCore.Http;

namespace Detours.Services;

public class ClaimProvider : IClaimProvider
{
	private static string GetClaimStrValue(ClaimsPrincipal claimsPrincipal
		, string claimId)
	{
		return claimsPrincipal.FindFirst(claimId)?.Value
		       ?? throw new InvalidOperationException($"Claim '{claimId}' was not found");
	}

	private static Guid GetClaimGuidValue(ClaimsPrincipal claimsPrincipal
		, string claimId)
	{
		var claimValueStr = GetClaimStrValue(claimsPrincipal, CustomClaimTypes.UserId);
		if (!Guid.TryParse(claimValueStr, out Guid claimValueGuid))
		{
			throw new InvalidOperationException($"Claim '{claimId}' could not be parsed as Guid value");
		}

		return claimValueGuid;
	}

	private readonly HttpContext _httpContext;

	public ClaimProvider(IHttpContextAccessor httpContextAccessor)
	{
		ArgumentNullException.ThrowIfNull(httpContextAccessor?.HttpContext);

		_httpContext = httpContextAccessor.HttpContext;
	}

	public Guid GetUserId() => GetClaimGuidValue(_httpContext.User, CustomClaimTypes.UserId);
	public Guid GetJwtId() => GetClaimGuidValue(_httpContext.User, JwtRegisteredClaimNames.Jti);
}
