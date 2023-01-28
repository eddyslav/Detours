using System.Collections.Concurrent;

using MediatR;

namespace Detours.Mediatr;

public class StrategiesPublisher
{
	private class CustomMediator : Mediator
	{
		private readonly Func<IEnumerable<Func<INotification, CancellationToken, Task>>, INotification, CancellationToken, Task> _publish;

		public CustomMediator(ServiceFactory serviceFactory, Func<IEnumerable<Func<INotification, CancellationToken, Task>>, INotification, CancellationToken, Task> publish)
			: base(serviceFactory)
			=> _publish = publish;

		protected override Task PublishCore(IEnumerable<Func<INotification, CancellationToken, Task>> allHandlers, INotification notification, CancellationToken cancellationToken)
			=> _publish(allHandlers, notification, cancellationToken);
	}

	private readonly ServiceFactory _serviceFactory;

	private readonly IDictionary<PublishStrategy, IMediator> _publishStrategies;

	private const PublishStrategy _defaultStrategy = PublishStrategy.SyncContinueOnException;

	private Task ParallelWhenAll(IEnumerable<Func<INotification, CancellationToken, Task>> handlers, INotification notification, CancellationToken cancellationToken)
	{
		var tasks = new List<Task>();

		foreach (var handler in handlers)
		{
			tasks.Add(Task.Run(() => handler(notification, cancellationToken), cancellationToken));
		}

		return Task.WhenAll(tasks);
	}

	private Task ParallelWhenAny(IEnumerable<Func<INotification, CancellationToken, Task>> handlers, INotification notification, CancellationToken cancellationToken)
	{
		var tasks = new List<Task>();

		foreach (var handler in handlers)
		{
			tasks.Add(Task.Run(() => handler(notification, cancellationToken), cancellationToken));
		}

		return Task.WhenAny(tasks);
	}

	private Task ParallelNoWait(IEnumerable<Func<INotification, CancellationToken, Task>> handlers, INotification notification, CancellationToken cancellationToken)
	{
		foreach (var handler in handlers)
		{
			Task.Run(() => handler(notification, cancellationToken), cancellationToken);
		}

		return Task.CompletedTask;
	}

	private async Task AsyncContinueOnException(IEnumerable<Func<INotification, CancellationToken, Task>> handlers, INotification notification, CancellationToken cancellationToken)
	{
		var tasks = new List<Task>();
		var exceptions = new List<Exception>();

		foreach (var handler in handlers)
		{
			try
			{
				tasks.Add(handler(notification, cancellationToken));
			}
			catch (Exception ex) when (ex is not OutOfMemoryException or StackOverflowException)
			{
				exceptions.Add(ex);
			}
		}

		try
		{
			await Task.WhenAll(tasks).ConfigureAwait(false);
		}
		catch (AggregateException ex)
		{
			exceptions.AddRange(ex.Flatten().InnerExceptions);
		}
		catch (Exception ex) when (ex is not OutOfMemoryException or StackOverflowException)
		{
			exceptions.Add(ex);
		}

		if (exceptions.Any())
		{
			throw new AggregateException(exceptions);
		}
	}

	private async Task SyncStopOnException(IEnumerable<Func<INotification, CancellationToken, Task>> handlers, INotification notification, CancellationToken cancellationToken)
	{
		foreach (var handler in handlers)
		{
			await handler(notification, cancellationToken).ConfigureAwait(false);
		}
	}

	private async Task SyncContinueOnException(IEnumerable<Func<INotification, CancellationToken, Task>> handlers, INotification notification, CancellationToken cancellationToken)
	{
		var exceptions = new List<Exception>();

		foreach (var handler in handlers)
		{
			try
			{
				await handler(notification, cancellationToken).ConfigureAwait(false);
			}
			catch (AggregateException ex)
			{
				exceptions.AddRange(ex.Flatten().InnerExceptions);
			}
			catch (Exception ex) when (ex is not OutOfMemoryException or StackOverflowException)
			{
				exceptions.Add(ex);
			}
		}

		if (exceptions.Any())
		{
			throw new AggregateException(exceptions);
		}
	}

	public StrategiesPublisher(ServiceFactory serviceFactory)
	{
		ArgumentNullException.ThrowIfNull(serviceFactory);

		_serviceFactory = serviceFactory;

		_publishStrategies = new ConcurrentDictionary<PublishStrategy, IMediator>();

		_publishStrategies[PublishStrategy.Async] = new CustomMediator(_serviceFactory, AsyncContinueOnException);
		_publishStrategies[PublishStrategy.ParallelNoWait] = new CustomMediator(_serviceFactory, ParallelNoWait);
		_publishStrategies[PublishStrategy.ParallelWhenAll] = new CustomMediator(_serviceFactory, ParallelWhenAll);
		_publishStrategies[PublishStrategy.ParallelWhenAny] = new CustomMediator(_serviceFactory, ParallelWhenAny);
		_publishStrategies[PublishStrategy.SyncContinueOnException] = new CustomMediator(_serviceFactory, SyncContinueOnException);
		_publishStrategies[PublishStrategy.SyncStopOnException] = new CustomMediator(_serviceFactory, SyncStopOnException);
	}

	public Task Publish<TNotification>(TNotification notification, PublishStrategy strategy, CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(notification);

		if (!_publishStrategies.TryGetValue(strategy, out var mediator))
		{
			throw new ArgumentException($"Unknown strategy: {strategy}");
		}

		return mediator.Publish(notification, cancellationToken);
	}

	public Task Publish<TNotification>(TNotification notification)
	{
		return Publish(notification, _defaultStrategy, default);
	}

	public Task Publish<TNotification>(TNotification notification, PublishStrategy strategy)
	{
		return Publish(notification, strategy, default);
	}

	public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken)
	{
		return Publish(notification, _defaultStrategy, cancellationToken);
	}
}
