namespace DDDBase.Models;

/// <summary>
/// Identifies an object as an aggregate root
/// </summary>
public interface IAggregateRoot : IEntity
{
  /// <summary>
  /// Version of the aggregate, this increases with every change to the aggregate
  /// </summary>
  long Version { get; }

  /// <summary>
  /// Collection of currently applied but unsaved events
  /// </summary>
  IReadOnlyCollection<IDomainEvent> Events { get; }

  /// <summary>
  /// Clears all events from the aggregate
  /// </summary>
  void MarkEventsCommitted();
}