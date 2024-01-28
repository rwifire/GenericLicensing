using FluentAssertions;
using GenericLicensing.Domain.Aggregates;
using GenericLicensing.Domain.Entities;
using GenericLicensing.Domain.Events;
using GenericLicensing.Domain.Events.License;
using GenericLicensing.Domain.ValueObjects;
using GenericLicensing.TestUtils;

namespace GenericLicensing.Domain.Tests.License;

public class LicenseTests
{
  [Fact]
  public void CreateLicenseCausesCorrectChange()
  {
    var key = new LicenseKey("some key");
    var licenseOwner = LicenseOwnerTestModel.CreateLicenseOwner();
    var licensedProduct = ProductTestModel.CreateProduct();
    var now = DateTime.UtcNow;
    var license = LicenseAggregate.Create(key, licenseOwner, licensedProduct);

    license.Version.Should().Be(1);
    license.IsDeleted.Should().BeFalse();
    license.LicenseKey.Should().Be(key);
    license.LicenseState.Should().Be(LicenseState.Active);
    license.LicenseOwner.LicenseOwnerId.Should().Be(licenseOwner.LicenseOwnerId);
    license.LicenseOwner.CompanyName.Should().Be(license.LicenseOwner.CompanyName);
    license.Product.ProductId.Should().Be(licensedProduct.ProductId);
    license.Product.ProductName.Should().Be(licensedProduct.ProductName);
    license.CreationDate.Should().BeWithin(TimeSpan.FromMilliseconds(500)).After(now);
  }

  [Fact]
  public void CreateLicenseCausesCorrectEvents()
  {
    var now = DateTime.UtcNow;
    var key = new LicenseKey("some key");
    var licenseOwner = LicenseOwnerTestModel.CreateLicenseOwner();
    var licensedProduct = ProductTestModel.CreateProduct();
    var license = LicenseAggregate.Create(key, licenseOwner, licensedProduct);

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
    @event.LicenseOwner.Should().NotBeNull();
    @event.LicenseOwner.LicenseOwnerId.Should().Be(license.LicenseOwner.LicenseOwnerId);
    @event.LicenseOwner.CompanyName.Should().Be(license.LicenseOwner.CompanyName);
    @event.Product.ProductId.Should().Be(licensedProduct.ProductId);
    @event.Product.ProductName.Should().Be(licensedProduct.ProductName);
    @event.Timestamp.Should().Be(license.CreationDate);
  }

  [Fact]
  public void DeleteLicenseCausesCorrectChange()
  {
    var key = new LicenseKey("some Key");
    var licenseOwner = LicenseOwnerTestModel.CreateLicenseOwner();
    var licensedProduct = ProductTestModel.CreateProduct();
    var license = LicenseAggregate.Create(key, licenseOwner, licensedProduct);

    license.Delete();

    license.Version.Should().Be(2);
    license.IsDeleted.Should().BeTrue();
  }

  [Fact]
  public void DeleteLicenseCausesCorrectEvent()
  {
    var key = new LicenseKey("some Key");
    var licenseOwner = LicenseOwnerTestModel.CreateLicenseOwner();
    var licensedProduct = ProductTestModel.CreateProduct();
    var license = LicenseAggregate.Create(key, licenseOwner, licensedProduct);

    license.Delete();

    var e = license.Events.Last() as LicenseDeletedEvent;
    Assert.NotNull(e);
    e.AggregateVersion.Should().Be(license.Version);
  }
}