using DDDBase.Models;
using GenericLicensing.Domain.Events;
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
  /// Indicates whether this License is deleted
  /// </summary>
  public bool IsDeleted { get; private set; }

  //used for serialization and deserialization
  private License()
  {
  }

  private License(Guid id, LicenseKey licenseKey)
  {
    ApplyChange(new LicenseCreatedEvent(id, licenseKey, LicenseState.Active, NextVersion()));
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
  public static License Create(LicenseKey key)
  {
    return new License(Guid.NewGuid(), key);
  }
}

public enum LicenseState
{
  Active,
  Inactive
}