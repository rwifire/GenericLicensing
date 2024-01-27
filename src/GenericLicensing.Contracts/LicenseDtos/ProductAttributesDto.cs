using System.ComponentModel.DataAnnotations;

namespace GenericLicensing.Contracts.LicenseDtos;

public class ProductAttributesDto
{
  [Required] public Dictionary<string, bool> Flags { get; set; }
  [Required] public Dictionary<string, int> Options { get; set; }
  [Required] public Dictionary<string, string> Configs { get; set; }
}