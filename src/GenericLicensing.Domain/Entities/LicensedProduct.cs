using Dawn;
using GenericLicensing.Domain.ValueObjects;

namespace GenericLicensing.Domain.Entities;

/// <summary>
/// Product Information that is licensed
/// </summary>
public class LicensedProduct
{
  /// <summary>
  /// ID of the product (product number or similar)
  /// </summary>
  public ProductId ProductId { get; }

  /// <summary>
  /// Name of the product
  /// </summary>
  public string ProductName { get; }

  public ProductAttributes Attributes { get; set; }

  public LicensedProduct(ProductId productId, string productName, ProductAttributes attributes)
  {
    ProductId = Guard.Argument(productId, nameof(productId)).NotNull();
    Attributes = Guard.Argument(attributes, nameof(attributes)).NotNull();
    ProductName = Guard.Argument(productName, nameof(productName)).NotEmpty().NotWhiteSpace();
  }
}