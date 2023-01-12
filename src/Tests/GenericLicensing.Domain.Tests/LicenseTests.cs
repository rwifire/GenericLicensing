using FluentAssertions;
using GenericLicensing.Domain;
using GenericLicensing.Domain.Aggregates;
using GenericLicensing.Domain.Events;
using GenericLicensing.Domain.ValueObjects;

namespace GenericLicensing.Domain.Tests;

public class LicenseTests
{
  [Fact]
  public void CreateLicenseCausesCorrectChange()
  {
    var key = new LicenseKey("some key");

    var license = License.Create(key);

    license.Version.Should().Be(1);
    license.IsDeleted.Should().BeFalse();
    license.LicenseKey.Should().Be(key);
    license.LicenseState.Should().Be(LicenseState.Active);
  }

  [Fact]
  public void CreateLicenseCausesCorrectEvents()
  {
    var now = DateTime.UtcNow;
    var key = new LicenseKey("some key");
    var license = License.Create(key);

    var events = license.Events;

    events.Count.Should().Be(1);
    var @event = events.First() as LicenseCreatedEvent;

    Assert.NotNull(@event);
    @event.AggregateId.Should().Be(license.Id);
    @event.AggregateVersion.Should().Be(license.Version);
    @event.Timestamp.Should().BeIn(DateTimeKind.Utc);
    @event.Timestamp.Should().BeWithin(TimeSpan.FromMilliseconds(100)).After(now);
    @event.LicenseKey.Should().Be(license.LicenseKey);
    @event.LicenseState.Should().Be(license.LicenseState);
  }

  [Fact]
  public void DeleteLicenseCausesCorrectChange()
  {
    var key = new LicenseKey("some Key");
    var license = License.Create(key);

    license.Delete();

    license.Version.Should().Be(2);
    license.IsDeleted.Should().BeTrue();
  }

  [Fact]
  public void DeleteLicenseCausesCorrectEvent()
  {
    var key = new LicenseKey("some Key");
    var license = License.Create(key);

    license.Delete();

    var e = license.Events.Last() as LicenseDeletedEvent;
    Assert.NotNull(e);
    e.AggregateVersion.Should().Be(license.Version);
  }
}