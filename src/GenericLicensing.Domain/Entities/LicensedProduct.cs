using GenericLicensing.Domain.ValueObjects;

namespace GenericLicensing.Domain.Entities;

public class LicensedProduct
{
  public ProductId ProductId { get; }
  public string ProductName { get; }

  public LicensedProduct(ProductId productId, string productName)
  {
    ProductId = productId;
    ProductName = productName;
  }
}