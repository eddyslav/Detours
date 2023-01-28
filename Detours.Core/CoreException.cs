namespace Detours.Core;

public abstract class CoreException : Exception
{
	public abstract ErrorCode ErrorCode { get; }

	protected CoreException(string message)
		: base(message)
	{
		
	}
}
