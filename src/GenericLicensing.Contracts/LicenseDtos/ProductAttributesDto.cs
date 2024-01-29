using System.ComponentModel.DataAnnotations;

namespace GenericLicensing.Contracts.LicenseDtos;

public class ProductAttributesDto
{
  [Required] public required Dictionary<string, bool> Flags { get; set; }
  [Required] public required Dictionary<string, int> Options { get; set; }
  [Required] public required Dictionary<string, string> Configs { get; set; }
}