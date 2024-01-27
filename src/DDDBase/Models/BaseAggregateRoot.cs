using System.Collections.Immutable;
using System.Reflection;

namespace DDDBase.Models;

public abstract class BaseAggregateRoot<TA> : IAggregateRoot
{
  private readonly Queue<IDomainEvent> _changes = new();

  /// <inheritdoc/>
  public abstract Guid Id { get; }

  /// <inheritdoc/>
  public long Version { get; private set; }

  /// <inheritdoc/>
  public IReadOnlyCollection<IDomainEvent> Events => _changes.ToImmutableArray();

  /// <inheritdoc/>
  public void MarkEventsCommitted()
  {
    _changes.Clear();
  }

  /// <summary>
  /// Gets the next version for this aggregate
  /// </summary>
  /// <returns>the next version for this aggregate</returns>
  protected long NextVersion()
  {
    return Version + 1;
  }

  protected void ApplyChange(IDomainEvent @event)
  {
    ApplyChange(@event, true);
  }

  private void ApplyChange(IDomainEvent @event, bool isNew)
  {
    this.AsDynamic().Apply(@event);
    Version++;
    if (isNew)
    {
      _changes.Enqueue(@event);
    }
  }

  /// <summary>
  /// Methods for Aggregate Creation via reflection
  /// </summary>

  #region Factory

  // ReSharper disable once StaticMemberInGenericType
  private static readonly ConstructorInfo CTor;

  /// <summary>
  /// Set constructor info for the generic parameter TA, used for aggregate creation
  /// </summary>
  /// <exception cref="AggregateException"></exception>
  static BaseAggregateRoot()
  {
    var aggregateType = typeof(TA);
    var ctorInfo = aggregateType.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
      null, new Type[0], new ParameterModifier[0]);
    if (null == ctorInfo)
    {
      throw new AggregateException(
        $"Unable to find required private parameterless constructor for Aggregate of type '{aggregateType.Name}'");
    }

    CTor = ctorInfo;
  }

  /// <summary>
  /// Creates an aggregate of the type TA, given as generic parameter, by applying the events from the method call
  /// </summary>
  /// <param name="events">Events that shall apply at creation</param>
  /// <returns>The created object of type TA</returns>
  /// <exception cref="ArgumentNullException">Thrown when no events given</exception>
  public static TA Create(IEnumerable<IDomainEvent> events)
  {
    var domainEvents = events.ToList().OrderBy(e => e.AggregateVersion);
    if (null == events || !domainEvents.Any())
    {
      throw new ArgumentNullException(nameof(events));
    }

    //Create object of type TA
    var result = (TA) CTor.Invoke(new object[0]);

    //Apply each event to the object after creation
    if (result is BaseAggregateRoot<TA> baseAggregate)
    {
      foreach (var @event in domainEvents)
      {
        baseAggregate.ApplyChange(@event, false);
      }
    }

    return result;
  }

  #endregion Factory
}