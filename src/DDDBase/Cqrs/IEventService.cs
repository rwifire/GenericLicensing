using DDDBase.Models;

namespace DDDBase.Cqrs;

public interface IEventService<TA> where TA : class, IAggregateRoot
{
  /// <summary>
  /// Persists all open events of an aggregate
  /// </summary>
  /// <param name="aggregateRoot">aggregate of which the events need to persisted</param>
  Task PersistAsync(TA aggregateRoot);

  /// <summary>
  /// Loads an aggregate from the event source by replaying all events
  /// </summary>
  /// <param name="aggregateId">Primary identifier of the aggregate</param>
  Task<TA?> LoadAsync(Guid aggregateId);
}