namespace Detours.Mediatr;

public enum PublishStrategy
{
	SyncContinueOnException = 0,
	SyncStopOnException,
	Async,
	ParallelNoWait,
	ParallelWhenAll,
	ParallelWhenAny,
}
