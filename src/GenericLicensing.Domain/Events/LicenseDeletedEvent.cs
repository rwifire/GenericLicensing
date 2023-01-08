using DDDBase.Models;

namespace GenericLicensing.Domain.Events;

public class LicenseDeletedEvent : BaseDomainEvent
{
  public LicenseDeletedEvent(Guid id, long version) : base(id, version)
  {
  }
}