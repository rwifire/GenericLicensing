using System.ComponentModel.DataAnnotations;

namespace GenericLicensing.Contracts.LicenseDtos;

public class LicensedProductDetailsDto
{
  [Required] public string ProductId { get; set; }
  [Required] public string ProductName { get; set; }
}