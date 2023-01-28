using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

using Microsoft.Extensions.Options;

using Microsoft.EntityFrameworkCore;

using Microsoft.IdentityModel.Tokens;

using LazyCache;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

using Detours.Core;

using Detours.Data;
using Detours.Data.Options;
using Detours.Data.Entities;
using Detours.Data.Models.Requests;
using Detours.Data.Models.Responses;

using Detours.Mediatr;

using Detours.Services.Utils;
using Detours.Services.Extensions;
using Detours.Services.Notifications;

namespace Detours.Services;

public class UserService : IUserService
{
	private readonly DetoursDbContext _dbContext;
	private readonly IClaimProvider _claimProvider;
	private readonly AuthenticationConfiguration _configuration;

	private readonly StrategiesPublisher _publisher;

	private readonly AsyncLazy<User> _currentUserLazy;

	private const string PhotoFileTemplate = "user-{0}-{1}.jpg";
	private const string DefaultPhotoName = "default.jpg";

	private TokenResponse GenerateToken(User user)
	{
		var tokenHandler = new JwtSecurityTokenHandler();

		var jwtId = Guid.NewGuid();
		var now = DateTime.UtcNow;
		var expiresAt = now.Add(_configuration.TokenLifetime);
		var key = Encoding.UTF8.GetBytes(_configuration.SecretKey);

		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Subject = new(new Claim[]
			{
				new(JwtRegisteredClaimNames.Sub, user.Email),
				new(JwtRegisteredClaimNames.Email, user.Email),
				new(JwtRegisteredClaimNames.Jti, jwtId.ToString()),
				new(CustomClaimTypes.UserId, user.Id.ToString()),
			}),
			IssuedAt = now,
			NotBefore = now,
			Expires = expiresAt,
			Issuer = _configuration.Issuer,
			Audience = _configuration.Issuer,
			SigningCredentials =
				new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
		};

		var token = tokenHandler.CreateToken(tokenDescriptor);

		var refreshToken = new RefreshToken
		{
			User = user,
			JwtId = jwtId,
			ExpiresAt = now.Add(_configuration.RefreshTokenLifetime),
			CreatedAt = now,
		};
		_dbContext.RefreshTokens.Add(refreshToken);

		return new TokenResponse
		{
			Token = tokenHandler.WriteToken(token),
			RefreshToken = refreshToken.Id,
			ExpiresAt = expiresAt,
		};
	}

	private async Task UpdateRefreshTokenAsync(RefreshTokenRequest request
		, Action<RefreshToken> updateAction
		, CancellationToken cancellationToken)
	{
		var jwtId = _claimProvider.GetJwtId();
		var refreshTokenId = request.RefreshToken;

		var refreshToken = await _dbContext.RefreshTokens
			.FirstOrDefaultAsync(x => x.Id == refreshTokenId && x.JwtId == jwtId, cancellationToken);

		if (refreshToken is null)
		{
			throw new ServiceArgumentException("Refresh token does not exist");
		}

		if (refreshToken.ExpiresAt < DateTime.UtcNow)
		{
			throw new ServiceArgumentException("Refresh token is expired");
		}

		if (refreshToken.Used)
		{
			throw new ServiceArgumentException("Refresh token was used");
		}

		if (refreshToken.Invalidated)
		{
			throw new ServiceArgumentException("Refresh token was invalidated");
		}

		updateAction(refreshToken);
		_dbContext.RefreshTokens.Update(refreshToken);
	}

	private async Task<User> GetCurrentUserImplAsync(CancellationToken cancellationToken)
	{
		var currentUserId = _claimProvider.GetUserId();

		var currentUser = await _dbContext.Users
			.FirstOrDefaultAsync(x => x.Id == currentUserId, cancellationToken);

		if (currentUser is null)
		{
			throw new InvalidOperationException($"User with id {currentUserId} does not exist");
		}
		
		return currentUser;
	}

	public UserService(DetoursDbContext dbContext
		, IClaimProvider claimProvider
		, IOptions<AuthenticationConfiguration> options
		, StrategiesPublisher publisher)
	{
		ArgumentNullException.ThrowIfNull(dbContext);
		ArgumentNullException.ThrowIfNull(claimProvider);

		ArgumentNullException.ThrowIfNull(publisher);

		_dbContext = dbContext;
		_claimProvider = claimProvider;
		_configuration = options.GetConfiguration();

		_publisher = publisher;

		_currentUserLazy = new AsyncLazy<User>(() => GetCurrentUserImplAsync(default));
	}

	public async Task<User> GetCurrentUserAsync(CancellationToken cancellationToken)
	{
		if (!_currentUserLazy.IsValueCreated)
		{
			cancellationToken.ThrowIfCancellationRequested();
		}

		return await _currentUserLazy.Value;
	}

	public async Task<TokenResponse> LoginUserAsync(LoginUserRequest request, CancellationToken cancellationToken)
	{
		var email = request.Email.ToLower();
		var user = await _dbContext.Users
			.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);

		if (user is null || !PasswordHasher.EqualPasswords(request.Password, user.Password))
		{
			throw new ServiceArgumentException("Email or passwords does not match");
		}

		var token = GenerateToken(user);
		await _dbContext.SaveChangesAsync(cancellationToken);

		return token;
	}

	public async Task<TokenResponse> RegisterUserAsync(RegisterUserRequest request, CancellationToken cancellationToken)
	{
		if (request.Photo is not null
			&& !request.Photo.ContentType.Contains("image"))
		{
			throw new ServiceArgumentException("Photo must be of type image");
		}

		var email = request.Email.ToLower();
		var isUserAlready = await _dbContext.Users
			.Where(user => user.Email == email)
			.AnyAsync(cancellationToken);

		if (isUserAlready)
		{
			throw new ServiceArgumentException("User with that email already exists");
		}

		var newUser = new User
		{
			Id = Guid.NewGuid(),
			Name = request.Name,
			Email = request.Email.ToLower(),
			Role = UserRole.User,
			Password = PasswordHasher.HashPassword(request.Password),
			Photo = DefaultPhotoName,
		};

		if (request.Photo is not null)
		{
			var img = await Image.LoadAsync(request.Photo.OpenReadStream(), cancellationToken);
			img.Mutate(x => x.Resize(500, 500));

			var imageFileName = string.Format(PhotoFileTemplate, newUser.Id, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
			await img.SaveAsJpegAsync($"public/img/users/{imageFileName}", cancellationToken);

			newUser.Photo = imageFileName;
		}

		_dbContext.Users.Add(newUser);

		var token = GenerateToken(newUser);
		await _dbContext.SaveChangesAsync(cancellationToken);

		var notification = new SignedUpUserNotification
		{
			UserName = newUser.Name,
			UserEmail = newUser.Email,
		};

		await _publisher.Publish(notification, PublishStrategy.ParallelNoWait, cancellationToken);

		return token;
	}

	public async Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken)
	{
		await UpdateRefreshTokenAsync(
			request,
			(refreshToken) => refreshToken.Used = true,
			cancellationToken);

		var currentUser = await GetCurrentUserAsync(cancellationToken);
		var token = GenerateToken(currentUser);

		await _dbContext.SaveChangesAsync(cancellationToken);

		return token;
	}
}
