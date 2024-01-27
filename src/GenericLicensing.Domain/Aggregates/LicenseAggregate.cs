using DDDBase.Models;
using GenericLicensing.Domain.Entities;
using GenericLicensing.Domain.Events;
using GenericLicensing.Domain.Events.License;
using GenericLicensing.Domain.ValueObjects;

namespace GenericLicensing.Domain.Aggregates;

public class LicenseAggregate : BaseAggregateRoot<LicenseAggregate>
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
  /// Information about the Product that this license gives usage rights to
  /// </summary>
  public LicensedProduct LicensedProduct { get; private set; }

  /// <summary>
  /// Time when this license was created
  /// </summary>
  public DateTime CreationDate { get; private set; }

  /// <summary>
  /// Indicates whether this License is deleted
  /// </summary>
  public bool IsDeleted { get; private set; }

  //used for serialization and deserialization
  private LicenseAggregate()
  {
  }

  private LicenseAggregate(Guid id, LicenseKey licenseKey, LicenseOwner licenseOwner, LicensedProduct licensedProduct)
  {
    ApplyChange(new LicenseCreatedEvent(id, licenseKey, licenseOwner, licensedProduct, LicenseState.Active,
      NextVersion()));
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
    LicensedProduct = e.LicensedProduct;
    LicenseState = e.LicenseState;
    IsDeleted = false;
    CreationDate = e.Timestamp;
  }

  private void Apply(LicenseDeletedEvent e)
  {
    IsDeleted = true;
  }

  /// <summary>
  /// Creates a new aggregate of the type <see cref="GrantAggregate"/>
  /// </summary>
  public static LicenseAggregate Create(LicenseKey key, LicenseOwner owner, LicensedProduct licensedProduct)
  {
    return new LicenseAggregate(Guid.NewGuid(), key, owner, licensedProduct);
  }
}

public enum LicenseState
{
  Active,
  Inactive
}