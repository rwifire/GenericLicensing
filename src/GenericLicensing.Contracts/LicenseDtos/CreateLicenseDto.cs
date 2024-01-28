using System.ComponentModel.DataAnnotations;

namespace GenericLicensing.Contracts.LicenseDtos;

public class CreateLicenseDto
{
  [Required] public LicenseOwnerDetailsDto LicenseOwner { get; set; }
  [Required] public ProductDetailsDto Product { get; set; }
}