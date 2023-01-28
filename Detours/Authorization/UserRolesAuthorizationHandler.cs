using Microsoft.AspNetCore.Authorization;

using ILogger = Serilog.ILogger;

using Detours.Services;

namespace Detours.Authorization;

internal sealed class UserRolesAuthorizationHandler : AuthorizationHandler<UserRolesRequirement>
{
	private readonly IUserService _userService;

	private readonly ILogger _logger;

	protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context
		, UserRolesRequirement requirement)
	{
		if (!(context.User?.Identity?.IsAuthenticated ?? false))
		{
			_logger.Warning("User is not authenticated");
			return;
		}
        
		var currentUser = await _userService.GetCurrentUserAsync(default);
		if (requirement.Roles.HasFlag(currentUser.Role))
		{
			context.Succeed(requirement);
			return;
		}

		_logger.Warning(
			"User has insufficient roles: [{UserRole}]. Required roles: [{RequiredUserRoles}]"
			, currentUser.Role
			, requirement.Roles);
	}

	public UserRolesAuthorizationHandler(IUserService userService, ILogger logger)
	{
		ArgumentNullException.ThrowIfNull(userService);
		ArgumentNullException.ThrowIfNull(logger);

		_userService = userService;
		_logger = logger.ForContext<UserRolesAuthorizationHandler>();
	}
}
