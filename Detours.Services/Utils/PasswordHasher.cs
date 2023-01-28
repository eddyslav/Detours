namespace Detours.Services.Utils;

internal static class PasswordHasher
{
	private const int WorkFactor = 12;

	public static bool EqualPasswords(string passwordToCompare, string passwordHashed)
	{
		return BCrypt.Net.BCrypt.EnhancedVerify(passwordToCompare, passwordHashed);
	}

	public static string HashPassword(string password)
	{
		if (string.IsNullOrWhiteSpace(password))
		{
			throw new ArgumentException($"\"{nameof(password)}\" cannot be null or empty");
		}

		return BCrypt.Net.BCrypt.EnhancedHashPassword(password, WorkFactor);
	}
}
