using System.Reflection;
using System.Text;
using DDDBase.Models;
using FakeItEasy;
using FluentAssertions;
using GenericLicensing.Persistence.Cosmos.EventStore;
using Newtonsoft.Json;

namespace GenericLicensing.Infrastructure.Tests;

public class EventSerializerTest
{
  private readonly byte[] _data;
  private readonly IDomainEvent _event;

  public EventSerializerTest()
  {
    _event = new MockEvent(Guid.NewGuid(), "Something", 1);
    _data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(_event));
  }

  [Fact]
  public void Deserialize_creates_correct_events()
  {
    var expectedEvent = _event;
    var sut = new EventSerializer(new[] {typeof(MockEvent).Assembly});

    var @event = sut.Deserialize(typeof(MockEvent).FullName, _data);

    @event.Should().BeOfType<MockEvent>();
    @event.Should().BeEquivalentTo(expectedEvent);
  }

  [Fact]
  public void Serialize_creates_correct_data()
  {
    var expectedData = _data;
    var sut = new EventSerializer(new[] {typeof(MockEvent).Assembly});

    var data = sut.Serialize(_event);

    data.Should().BeEquivalentTo(expectedData);
  }

  [Fact]
  public void Deserialize_throws_exception_for_unknown_type()
  {
    var sut = new EventSerializer(new[] {typeof(MockEvent).Assembly});
    var badTypeString = "Some.Unknown.Type";

    var act = () => { sut.Deserialize(badTypeString, _data); };

    act.Should().Throw<ArgumentOutOfRangeException>().And.Message.Should().Contain(badTypeString);
  }

  [Fact]
  public void Deserialize_gets_type_from_cache()
  {
    var expectedEvent = _event;
    new EventSerializer(new[] {typeof(MockEvent).Assembly}).Deserialize(typeof(MockEvent).FullName, _data);
    var sut = new EventSerializer(A.Fake<IEnumerable<Assembly>>());

    var @event = sut.Deserialize(typeof(MockEvent).FullName, _data);

    @event.Should().BeEquivalentTo(expectedEvent);
  }
}