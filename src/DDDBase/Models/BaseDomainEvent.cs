namespace DDDBase.Models;

public abstract class BaseDomainEvent : IDomainEvent
{
  /// <summary>
  /// For Deserialization
  /// </summary>
  protected BaseDomainEvent()
  {
  }

  protected BaseDomainEvent(Guid aggregateId, long version)
  {
    AggregateId = aggregateId;
    AggregateVersion = version;
    Timestamp = DateTime.UtcNow;
  }

  /// <inheritdoc/>
  public long AggregateVersion { get; private set; }

  /// <inheritdoc/>
  public Guid AggregateId { get; private set; }

  /// <inheritdoc/>
  public DateTime Timestamp { get; private set; }
}