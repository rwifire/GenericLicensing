using System.ComponentModel.DataAnnotations;

namespace GenericLicensing.Contracts.LicenseDtos;

public class LicenseDetailsDto
{
  [Required] public string Id { get; set; }

  [Required] public string LicenseKey { get; set; }

  [Required] public string LicenseState { get; set; }

  [Required] public LicenseOwnerDetailsDto LicenseOwner { get; set; }

  [Required] public long Version { get; set; }
}