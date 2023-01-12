using DDDBase.Models;
using GenericLicensing.Domain.Aggregates;
using GenericLicensing.Domain.ValueObjects;

namespace GenericLicensing.Domain.Events;

public class LicenseCreatedEvent : BaseDomainEvent
{
  public LicenseKey LicenseKey { get; }
  public LicenseState LicenseState { get; }

  public LicenseCreatedEvent(Guid id, LicenseKey licenseKey, LicenseState licenseState, long version) : base(id,
    version)
  {
    LicenseKey = licenseKey;
    LicenseState = licenseState;
  }
}