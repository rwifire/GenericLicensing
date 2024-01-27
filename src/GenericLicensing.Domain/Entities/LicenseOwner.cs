using Dawn;
using GenericLicensing.Domain.ValueObjects;

namespace GenericLicensing.Domain.Entities;

public class LicenseOwner
{
  /// <summary>
  /// Unique reference to the license owner within a different system (CRM or ERP)
  /// </summary>
  public LicenseOwnerId LicenseOwnerId { get; private set; }

  /// <summary>
  /// Company Name of the owner of this license
  /// </summary>
  public string CompanyName { get; private set; }

  public LicenseOwner(LicenseOwnerId id, string companyName)
  {
    LicenseOwnerId = id;
    CompanyName = Guard.Argument(companyName, nameof(companyName)).NotEmpty().NotWhiteSpace();
  }
}