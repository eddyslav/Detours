namespace Detours.Core;

public class UnexpectedServiceResponseException : CoreException
{
	public override ErrorCode ErrorCode => ErrorCode.UnexpectedServiceResponse;

	public UnexpectedServiceResponseException(string message)
		: base(message) { }
}
