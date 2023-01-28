namespace Detours.Services;

public interface IClaimProvider
{
	Guid GetUserId();
	Guid GetJwtId();
}
