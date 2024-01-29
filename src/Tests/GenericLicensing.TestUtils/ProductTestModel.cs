using GenericLicensing.Domain.Entities;
using GenericLicensing.Domain.ValueObjects;

namespace GenericLicensing.TestUtils;

public class ProductTestModel
{
  /// <summary>
  /// Creates a Product object
  /// </summary>
  /// <param name="iD">randomized if not set</param>
  /// <param name="name">randomized if not set</param>
  /// <param name="flags">empty if not set</param>
  /// <param name="options">empty if not set</param>
  /// <param name="configs">empty if not set</param>
  /// <returns></returns>
  public static Product CreateProduct(string iD = "", string name = "", Dictionary<string, bool>? flags = null,
    Dictionary<string, int>? options = null, Dictionary<string, string>? configs = null)
  {
    var prodId = string.IsNullOrWhiteSpace(iD) ? new ProductId(Guid.NewGuid().ToString()) : new ProductId(iD);
    var prodName = string.IsNullOrWhiteSpace(name) ? "Product " + new Random().Next(1, 15) : name;

    var prodFlags = flags ?? new Dictionary<string, bool>();
    var prodOptions = options ?? new Dictionary<string, int>();
    var prodConfigs = configs ?? new Dictionary<string, string>();
    var attributes = new ProductAttributes(prodFlags, prodOptions, prodConfigs);

    return new Product(prodId, prodName, attributes);
  }
}