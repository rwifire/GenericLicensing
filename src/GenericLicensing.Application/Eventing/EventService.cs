using DDDBase.Cqrs;
using DDDBase.Models;
using GenericLicensing.Contracts.Persistence;
using Microsoft.Extensions.Logging;

namespace GenericLicensing.Application.Eventing;

/// <summary>
/// Makes sure that Aggregate Events are persisted and communicated
/// </summary>
/// <typeparam name="TA">Type of the aggregate</typeparam>
public class EventService<TA> : IEventService<TA> where TA : class, IAggregateRoot
{
  private readonly IEventRepository<TA> _eventRepository;
  private readonly ILogger<EventService<TA>> _logger;

  public EventService(ILogger<EventService<TA>> logger, IEventRepository<TA> eventRepository)
  {
    _logger = logger;
    _eventRepository = eventRepository;
  }

  public async Task PersistAsync(TA aggregateRoot)
  {
    if (!aggregateRoot.Events.Any())
    {
      _logger.LogWarning("Persist called without events on aggregate {AgRootId}", aggregateRoot.Id);
      return;
    }

    await _eventRepository.AppendAsync(aggregateRoot);
    _logger.LogInformation(
      "Stored {EventCount} Events for aggregate {AgRootId} in events source",
      aggregateRoot.Events.Count, aggregateRoot.Id);
  }

  public Task<TA?> LoadAsync(Guid aggregateId)
  {
    _logger.LogInformation("Restoring Aggregate with Id {AggregateId}", aggregateId);
    return _eventRepository.RestoreAggregate(aggregateId);
  }
}