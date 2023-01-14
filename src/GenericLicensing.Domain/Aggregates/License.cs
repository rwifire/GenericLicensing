using DDDBase.Models;
using GenericLicensing.Domain.Entities;
using GenericLicensing.Domain.Events;
using GenericLicensing.Domain.Events.License;
using GenericLicensing.Domain.ValueObjects;

namespace GenericLicensing.Domain.Aggregates;

public class License : BaseAggregateRoot<License>
{
  private Guid _id;

  /// <summary>
  /// Unique identifier of this aggregate within the domain
  /// </summary>
  public override Guid Id => _id;

  /// <summary>
  /// A unique key for this license that can follow certain rules and can be used as a product key for example
  /// </summary>
  public LicenseKey LicenseKey { get; private set; }

  /// <summary>
  /// The state of the license, is it active or inactive or in other states
  /// </summary>
  public LicenseState LicenseState { get; private set; }

  /// <summary>
  /// Information about the License Owner
  /// </summary>
  public LicenseOwner LicenseOwner { get; private set; }

  /// <summary>
  /// Indicates whether this License is deleted
  /// </summary>
  public bool IsDeleted { get; private set; }

  //used for serialization and deserialization
  private License()
  {
  }

  private License(Guid id, LicenseKey licenseKey, LicenseOwner licenseOwner)
  {
    ApplyChange(new LicenseCreatedEvent(id, licenseKey, licenseOwner, LicenseState.Active, NextVersion()));
  }

  /// <summary>
  /// Deletes this aggregate
  /// </summary>
  public void Delete()
  {
    ApplyChange(new LicenseDeletedEvent(_id, NextVersion()));
  }


  private void Apply(LicenseCreatedEvent e)
  {
    _id = e.AggregateId;
    LicenseKey = e.LicenseKey;
    LicenseOwner = e.LicenseOwner;
    LicenseState = e.LicenseState;
    IsDeleted = false;
  }

  private void Apply(LicenseDeletedEvent e)
  {
    IsDeleted = true;
  }

  /// <summary>
  /// Creates a new aggregate of the type <see cref="GrantAggregate"/>
  /// </summary>
  public static License Create(LicenseKey key, LicenseOwner owner)
  {
    return new License(Guid.NewGuid(), key, owner);
  }
}

public enum LicenseState
{
  Active,
  Inactive
}