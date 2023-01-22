using DDDBase.Models;

namespace GenericLicensing.Domain.ValueObjects;

public class ProductId : ValueObject
{
  public string Id { get; private set; }

  public ProductId(string productId)
  {
    Id = productId;
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