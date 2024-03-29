﻿using Dawn;
using DDDBase.Models;

namespace GenericLicensing.Domain.ValueObjects;

public class LicenseOwnerId : ValueObject
{
  public string Id { get; }

  public LicenseOwnerId(string id)
  {
    Id = Guard.Argument(id, nameof(id)).NotEmpty().NotWhiteSpace();
  }

  public override string ToString()
  {
    return Id;
  }

  protected override IEnumerable<object> GetEqualityComponents()
  {
    yield return Id;
  }
}