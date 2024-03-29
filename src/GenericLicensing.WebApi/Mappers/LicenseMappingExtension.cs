﻿using GenericLicensing.Contracts.LicenseDtos;
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
      Product = licenseAggregate.Product.ToProductDetails(),
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

  public static ProductDetailsDto ToProductDetails(this Product product)
  {
    return new ProductDetailsDto()
    {
      ProductId = product.ProductId.ToString(),
      ProductName = product.ProductName,
      ProductAttributes = product.Attributes.ToProductAttributesDto()
    };
  }

  public static ProductAttributesDto ToProductAttributesDto(this ProductAttributes attributes)
  {
    return new ProductAttributesDto()
    {
      Flags = attributes.Flags,
      Options = attributes.Options,
      Configs = attributes.Configs
    };
  }

  public static Product ToLicensedProduct(this ProductDetailsDto product)
  {
    var attributes = new ProductAttributes(product.ProductAttributes.Flags, product.ProductAttributes.Options,
      product.ProductAttributes.Configs);
    return new Product(new ProductId(product.ProductId), product.ProductName, attributes);
  }
}