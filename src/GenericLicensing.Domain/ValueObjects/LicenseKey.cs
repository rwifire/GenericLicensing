using DDDBase.Models;

namespace GenericLicensing.Domain.ValueObjects;

public class LicenseKey : ValueObject
{
  public string Key { get; private set; }

  public LicenseKey(string licenseKey)
  {
    Key = licenseKey;
  }

  public override string ToString()
  {
    return Key;
  }

  protected override IEnumerable<object> GetEqualityComponents()
  {
    yield return Key;
  }
}