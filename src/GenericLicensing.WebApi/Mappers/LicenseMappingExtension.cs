using GenericLicensing.Contracts.LicenseDtos;
using GenericLicensing.Domain.Aggregates;
using GenericLicensing.Domain.Entities;
using GenericLicensing.Domain.ValueObjects;

namespace GenericLicensing.WebApi.Mappers;

public static class LicenseMappingExtension
{
  public static LicenseDetailsDto ToLicenseDetails(this License license)
  {
    return new LicenseDetailsDto()
    {
      Id = license.Id.ToString(),
      LicenseKey = license.LicenseKey.ToString(),
      LicenseState = license.LicenseState.ToString(),
      LicenseOwner = license.LicenseOwner.ToLicenseOwnerDetails(),
      Version = license.Version
    };
  }

  public static LicenseOwnerDetailsDto ToLicenseOwnerDetails(this LicenseOwner owner)
  {
    return new LicenseOwnerDetailsDto()
    {
      LicenseOwnerId = owner.LicenseOwnerId.ToString(),
      CompanyName = owner.CompanyName
    };
  }

  public static LicenseOwner ToLicenseOwner(this LicenseOwnerDetailsDto owner)
  {
    return new LicenseOwner(new LicenseOwnerId(owner.LicenseOwnerId), owner.CompanyName);
  }
}