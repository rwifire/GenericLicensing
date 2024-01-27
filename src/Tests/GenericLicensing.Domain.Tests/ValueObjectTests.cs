using FluentAssertions;
using GenericLicensing.Domain.ValueObjects;

namespace GenericLicensing.Domain.Tests;

public class ValueObjectTests
{
  [Fact]
  public void LicenseKeyCreatedCorrect()
  {
    var key = "Some key";
    var licenseKey = new LicenseKey(key);

    licenseKey.Key.Should().Be(key);
  }

  [Fact]
  public void LicenseKeyEqualityCorrect()
  {
    var key = "Some key";
    var licenseKey = new LicenseKey(key);
    var key2 = "Some other key";
    var licenseKey2 = new LicenseKey(key2);
    var key3 = key;
    var licenseKey3 = new LicenseKey(key3);


    licenseKey.Should().NotBe(licenseKey2);
    (licenseKey != licenseKey2).Should().BeTrue();
    licenseKey.Should().Be(licenseKey3);
    (licenseKey == licenseKey3).Should().BeTrue();
  }

  [Fact]
  public void LicenseOwnerIdCreatedCorrect()
  {
    var id = "Some id";
    var licenseOwnerId = new LicenseOwnerId(id);

    licenseOwnerId.Id.Should().Be(id);
  }

  [Fact]
  public void LicenseOwnerIdEqualityCorrect()
  {
    var id = "Some id";
    var licenseOwnerId = new LicenseOwnerId(id);
    var id2 = "Some other id";
    var licenseOwnerId2 = new LicenseOwnerId(id2);
    var key3 = id;
    var licenseOwnerId3 = new LicenseOwnerId(key3);


    licenseOwnerId.Should().NotBe(licenseOwnerId2);
    (licenseOwnerId != licenseOwnerId2).Should().BeTrue();
    licenseOwnerId.Should().Be(licenseOwnerId3);
    (licenseOwnerId == licenseOwnerId3).Should().BeTrue();
  }
}