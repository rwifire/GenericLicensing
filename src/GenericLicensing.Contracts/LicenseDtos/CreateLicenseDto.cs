using System.ComponentModel.DataAnnotations;

namespace GenericLicensing.Contracts.LicenseDtos;

public class CreateLicenseDto
{
  [Required] public required LicenseOwnerDetailsDto LicenseOwner { get; set; }
  [Required] public required ProductDetailsDto Product { get; set; }
}