namespace GenericLicensing.Persistence.Cosmos.EventStore;

public class EventData
{
  public Guid Id { get; set; }

  public Guid AggregateId { get; set; }

  public long AggregateVersion { get; set; }

  public required string Type { get; set; }

  public required byte[] Data { get; set; }


  public static EventData Create(Guid aggregateId, long aggregateVersion, string type, byte[] data)
  {
    return new EventData()
    {
      Id = Guid.NewGuid(),
      AggregateId = aggregateId,
      AggregateVersion = aggregateVersion,
      Type = type,
      Data = data
    };
  }
}