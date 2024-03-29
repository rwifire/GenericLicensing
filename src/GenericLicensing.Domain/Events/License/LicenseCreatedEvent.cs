﻿using DDDBase.Models;
using GenericLicensing.Domain.Aggregates;
using GenericLicensing.Domain.Entities;
using GenericLicensing.Domain.ValueObjects;

namespace GenericLicensing.Domain.Events.License;

public class LicenseCreatedEvent : BaseDomainEvent
{
  public LicenseKey LicenseKey { get; }
  public LicenseOwner LicenseOwner { get; }
  public Product Product { get; }
  public LicenseState LicenseState { get; }

  public LicenseCreatedEvent(Guid id, LicenseKey licenseKey, LicenseOwner licenseOwner, Product product,
    LicenseState licenseState,
    long version) : base(id,
    version)
  {
    LicenseKey = licenseKey;
    LicenseOwner = licenseOwner;
    Product = product;
    LicenseState = licenseState;
  }
}