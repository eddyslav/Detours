using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Detours.Data.Mappings;
using Detours.Data.Models.Responses;

using Detours.Services;

namespace Detours.Controllers;

[Authorize]
[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
	private readonly IUserService _userService;

	public UsersController(IUserService userService)
	{
		ArgumentNullException.ThrowIfNull(userService);

		_userService = userService;
	}

	[HttpGet("me")]
	public async Task<UserResponse> GetCurrentUserAsync(CancellationToken cancellationToken)
	{
		var currentUser = await _userService.GetCurrentUserAsync(cancellationToken);
		return currentUser.ToResponse();
	}
}
