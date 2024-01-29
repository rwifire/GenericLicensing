using System.ComponentModel.DataAnnotations;

namespace GenericLicensing.Contracts.LicenseDtos;

public class ProductDetailsDto
{
  [Required] public required string ProductId { get; set; }
  [Required] public required string ProductName { get; set; }
  [Required] public required ProductAttributesDto ProductAttributes { get; set; }
}