using Detours.Data.Entities;
using Detours.Data.Models.Requests;
using Detours.Data.Models.Responses;

namespace Detours.Services;

public interface IUserService
{
	Task<User> GetCurrentUserAsync(CancellationToken cancellationToken);

	Task<TokenResponse> LoginUserAsync(LoginUserRequest request, CancellationToken cancellationToken);

	Task<TokenResponse> RegisterUserAsync(RegisterUserRequest request, CancellationToken cancellationToken);

	Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken);
}
