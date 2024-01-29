using System.Collections.Concurrent;
using System.Reflection;
using System.Text;
using DDDBase.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GenericLicensing.Persistence.Cosmos.EventStore;

///<inheritdoc cref="IEventSerializer"/>
public class EventSerializer : IEventSerializer
{
  private static readonly JsonSerializerSettings JsonSerializerSettings = new()
  {
    ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
    ContractResolver = new PrivateSetterContractResolver(),
    MissingMemberHandling = MissingMemberHandling.Error
  };

  private static readonly ConcurrentDictionary<string, Type> TypeCache = new();
  private readonly IEnumerable<Assembly> _assemblies;

  public EventSerializer(IEnumerable<Assembly> assemblies)
  {
    _assemblies = assemblies ?? new[] {Assembly.GetExecutingAssembly()};
  }

  ///<inheritdoc cref="IEventSerializer"/>
  public IDomainEvent Deserialize(string type, byte[] data)
  {
    var jsonData = Encoding.UTF8.GetString(data);
    var eventType = GetTypeFromAssemblies(type);
    var deserializedObject = JsonConvert.DeserializeObject(jsonData, eventType, JsonSerializerSettings);
    return (IDomainEvent) deserializedObject;
  }


  ///<inheritdoc cref="IEventSerializer"/>
  public byte[] Serialize(IDomainEvent @event)
  {
    var json = JsonConvert.SerializeObject((dynamic) @event);
    var data = Encoding.UTF8.GetBytes(json);
    return data;
  }

  private Type GetTypeFromAssemblies(string type)
  {
    if (TypeCache.ContainsKey(type))
    {
      return TypeCache[type];
    }

    var eventType = _assemblies.Select(a => a.GetType(type, false))
      .FirstOrDefault(t => t != null) ?? Type.GetType(type);
    if (null == eventType)
    {
      throw new ArgumentOutOfRangeException(nameof(type), $"No Type found in assemblies for name: {type}");
    }

    TypeCache.TryAdd(type, eventType);
    return eventType;
  }
}

/// <summary>
/// https://www.mking.net/blog/working-with-private-setters-in-json-net
/// </summary>
internal class PrivateSetterContractResolver : DefaultContractResolver
{
  protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
  {
    var jsonProperty = base.CreateProperty(member, memberSerialization);
    if (jsonProperty.Writable)
    {
      return jsonProperty;
    }

    if (member is PropertyInfo propertyInfo)
    {
      var setter = propertyInfo.GetSetMethod(true);
      jsonProperty.Writable = setter != null;
    }

    return jsonProperty;
  }
}

/// <summary>
/// Used to provide functionality to serialize and deserialize events
/// </summary>
public interface IEventSerializer
{
  /// <summary>
  /// Deserializes stored date into domain events
  /// </summary>
  /// <param name="type">type of the event that stored in the data</param>
  /// <param name="data">actual data of the event</param>
  /// <returns></returns>
  IDomainEvent Deserialize(string type, byte[] data);

  /// <summary>
  /// Serializes an event into its raw bytes for storage
  /// </summary>
  /// <param name="event">event to be serialized</param>
  /// <returns></returns>
  byte[] Serialize(IDomainEvent @event);
}