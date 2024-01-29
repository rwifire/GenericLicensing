using DDDBase.Models;

namespace DDDBase.Cqrs;

public interface IEventRepository<TA>
  where TA : class, IAggregateRoot
{
  /// <summary>
  /// Appends the events from an aggregate to its event stream persistence
  /// </summary>
  /// <param name="aggregateRoot">Aggregate that needs its events to be persisted</param>
  Task AppendAsync(TA aggregateRoot);

  /// <summary>
  /// Restores an aggregate from its persisted events
  /// </summary>
  /// <param name="key">Id of the aggregate that needs to be restored</param>
  Task<TA?> RestoreAggregate(Guid key);
}