namespace Detours.Core;

public class ServiceArgumentException : CoreException
{
	public override ErrorCode ErrorCode => ErrorCode.InvalidValue;
	
	public ServiceArgumentException(string message)
		: base(message)
	{
	}
}
