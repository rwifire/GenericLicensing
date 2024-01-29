using System.Collections.Concurrent;
using DDDBase.Cqrs;
using DDDBase.Models;
using FluentValidation;
using FluentValidation.Results;
using GenericLicensing.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GenericLicensing.Persistence.Cosmos.EventStore;

public class EventRepository<TA> : IEventRepository<TA>
  where TA : class, IAggregateRoot
{
  private static readonly ConcurrentDictionary<Guid, SemaphoreSlim> Locks = new();
  private readonly IGenericLicenseDbContext _dbContext;
  private readonly IEventSerializer _eventSerializer;
  private readonly ILogger<EventRepository<TA>> _logger;


  public EventRepository(IGenericLicenseDbContext dbContext, IEventSerializer eventSerializer,
    ILogger<EventRepository<TA>> logger)
  {
    _dbContext = dbContext;
    _logger = logger;
    _eventSerializer = eventSerializer;
  }

  public async Task AppendAsync(TA aggregateRoot)
  {
    _logger.LogInformation("Appending Events of Aggregate with DId {AggregateId}", aggregateRoot.Id);
    if (!aggregateRoot.Events.Any())
    {
      return;
    }

    //expected is one less than the current event
    var expectedDbVersion = aggregateRoot.Events.First().AggregateVersion - 1;

    //Create lock for this aggregate and wait for access
    var aggregateLock = Locks.GetOrAdd(aggregateRoot.Id, new SemaphoreSlim(1, 1));
    _logger.LogInformation("Waiting on Lock with Id: {Lock}", aggregateRoot.Id);
    await aggregateLock.WaitAsync();

    //get the last stored aggregate version
    var dbVersion = await _dbContext.Events
      .Where(e => e.AggregateId == aggregateRoot.Id)
      .MaxAsync(e => (long?) e.AggregateVersion);
    //make version check
    if (dbVersion != null && dbVersion != expectedDbVersion)
    {
      //TODO: Remove magic strings
      var exc = new ValidationException("Aggregate Version does not match.", new List<ValidationFailure>()
      {
        new(nameof(aggregateRoot.Version), "Expected Version to be " + expectedDbVersion, dbVersion)
      });
      throw exc;
    }

    try
    {
      _logger.LogInformation("Adding {EventCount} Events to the stream of {AggregateId}",
        aggregateRoot.Events.Count, aggregateRoot.Id);
      foreach (var @event in aggregateRoot.Events)
      {
        var data = _eventSerializer.Serialize(@event);
        var eventType = @event.GetType();
        var eventData = EventData.Create(aggregateRoot.Id, aggregateRoot.Version,
          eventType.AssemblyQualifiedName ?? throw new InvalidOperationException(), data);
        await _dbContext.Events.AddAsync(eventData);
      }
    }
    catch
    {
      await _dbContext.DisposeAsync();
      throw;
    }
    finally
    {
      aggregateLock.Release();
    }

    Locks.Remove(aggregateRoot.Id, out _);
  }

  public async Task<TA?> RestoreAggregate(Guid key)
  {
    _logger.LogInformation("Restoring aggregate with DId {Key}", key);
    var partitionKey = key.ToString();

    var events = await _dbContext.Events
      .Where(e => e.AggregateId == key)
      .Select(e => _eventSerializer.Deserialize(e.Type, e.Data)).ToListAsync();

    if (!events.Any())
    {
      _logger.LogInformation("No events found for aggregate with DId {Key}", key);
      return null;
    }

    _logger.LogInformation("Recreating Aggregate {Key} from {EventAmount} Events", key, events.Count);
    var result = BaseAggregateRoot<TA>.Create(events.OrderBy(e => e.AggregateVersion));
    return result;
  }
}