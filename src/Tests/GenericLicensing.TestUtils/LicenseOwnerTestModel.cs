using GenericLicensing.Domain.Entities;
using GenericLicensing.Domain.ValueObjects;

namespace GenericLicensing.TestUtils;

public class LicenseOwnerTestModel
{
  /// <summary>
  /// Creates a license owner object
  /// </summary>
  /// <param name="iD">randomized if not set</param>
  /// <param name="companyName">randomized if not set</param>
  /// <returns></returns>
  public static LicenseOwner CreateLicenseOwner(string iD = "", string companyName = "")
  {
    var licenseOwnerId = string.IsNullOrWhiteSpace(iD)
      ? new LicenseOwnerId(Guid.NewGuid().ToString())
      : new LicenseOwnerId(iD);
    var name = string.IsNullOrWhiteSpace(companyName) ? "Company " + new Random().Next(1, 15) : companyName;

    return new LicenseOwner(licenseOwnerId, name);
  }
}