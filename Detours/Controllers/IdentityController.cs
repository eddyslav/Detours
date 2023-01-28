using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Detours.Data.Models.Requests;
using Detours.Data.Models.Responses;

using Detours.Services;

namespace Detours.Controllers;

[ApiController]
[Route("api/users")]
public class IdentityController : ControllerBase
{
	private readonly IUserService _userService;

	public IdentityController(IUserService userService)
	{
		ArgumentNullException.ThrowIfNull(userService);
		
		_userService = userService;
	}

	[HttpPost("login")]
	public async Task<TokenResponse> LoginUserAsync([FromBody] LoginUserRequest request
		, CancellationToken cancellationToken)
	{
		return await _userService.LoginUserAsync(request, cancellationToken);
	}

	[HttpPost("register")]
	public async Task<TokenResponse> RegisterUserAsync([FromForm] RegisterUserRequest request
		, CancellationToken cancellationToken)
	{
		return await _userService.RegisterUserAsync(request, cancellationToken);
	}

	[Authorize]
	[HttpPost("refresh")]
	public async Task<TokenResponse> RefreshTokenAsync([FromBody] RefreshTokenRequest request
		, CancellationToken cancellationToken)
	{
		return await _userService.RefreshTokenAsync(request, cancellationToken);
	}
}
