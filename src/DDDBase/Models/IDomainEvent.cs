namespace DDDBase.Models;

public interface IDomainEvent
{
  /// <summary>
  /// Version of the aggregate after this event was applied
  /// </summary>
  long AggregateVersion { get; }

  /// <summary>
  /// Id of the aggregate that this event applies to
  /// </summary>
  Guid AggregateId { get; }

  /// <summary>
  /// Timestamp when this event was applied
  /// </summary>
  DateTime Timestamp { get; }
}