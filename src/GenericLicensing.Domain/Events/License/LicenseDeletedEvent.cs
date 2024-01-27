using DDDBase.Models;

namespace GenericLicensing.Domain.Events.License;

public class LicenseDeletedEvent : BaseDomainEvent
{
  public LicenseDeletedEvent(Guid id, long version) : base(id, version)
  {
  }
}