using System.ComponentModel.DataAnnotations;

namespace GenericLicensing.Contracts.LicenseDtos;

public class LicenseDetailsDto
{
  [Required] public required string Id { get; set; }

  [Required] public required string LicenseKey { get; set; }

  [Required] public required string LicenseState { get; set; }

  [Required] public required LicenseOwnerDetailsDto LicenseOwner { get; set; }

  [Required] public required ProductDetailsDto Product { get; set; }

  [Required] public required long Version { get; set; }
}