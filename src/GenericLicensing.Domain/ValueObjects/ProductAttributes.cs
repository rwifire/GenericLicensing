using Dawn;
using DDDBase.Models;

namespace GenericLicensing.Domain.ValueObjects;

/// <summary>
/// Attributes that are relevant for this concrete products. Examples:
/// Flag: AllowsOfflineInstallation => True
/// Option: AllowedDevices => 1000
/// Config: OnlineActivationUri => https://example.de
/// </summary>
public class ProductAttributes : ValueObject
{
  public Dictionary<string, bool> Flags { get; }
  public Dictionary<string, int> Options { get; }
  public Dictionary<string, string> Configs { get; }

  public ProductAttributes(Dictionary<string, bool> flags, Dictionary<string, int> options,
    Dictionary<string, string> configs)
  {
    Flags = Guard.Argument(flags, nameof(flags)).NotNull();
    Options = Guard.Argument(options, nameof(options)).NotNull();
    Configs = Guard.Argument(configs, nameof(configs)).NotNull();
  }

  protected override IEnumerable<object> GetEqualityComponents()
  {
    yield return Flags;
    yield return Options;
    yield return Configs;
  }
}