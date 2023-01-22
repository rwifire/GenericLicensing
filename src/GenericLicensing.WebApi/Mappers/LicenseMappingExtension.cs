using GenericLicensing.Contracts.LicenseDtos;
using GenericLicensing.Domain.Aggregates;
using GenericLicensing.Domain.Entities;
using GenericLicensing.Domain.ValueObjects;

namespace GenericLicensing.WebApi.Mappers;

public static class LicenseMappingExtension
{
  public static LicenseDetailsDto ToLicenseDetails(this LicenseAggregate licenseAggregate)
  {
    return new LicenseDetailsDto()
    {
      Id = licenseAggregate.Id.ToString(),
      LicenseKey = licenseAggregate.LicenseKey.ToString(),
      LicenseState = licenseAggregate.LicenseState.ToString(),
      LicenseOwner = licenseAggregate.LicenseOwner.ToLicenseOwnerDetails(),
      Version = licenseAggregate.Version
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

  public static LicensedProductDetailsDto ToLicensedProductDetails(this LicensedProduct product)
  {
    return new LicensedProductDetailsDto()
    {
      ProductId = product.ProductId.ToString(),
      ProductName = product.ProductName
    };
  }

  public static LicensedProduct ToLicensedProduct(this LicensedProductDetailsDto product)
  {
    return new LicensedProduct(new ProductId(product.ProductId), product.ProductName);
  }
}