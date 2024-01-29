using System.ComponentModel.DataAnnotations;

namespace GenericLicensing.Contracts.LicenseDtos;

public class LicenseOwnerDetailsDto
{
  [Required] public required string LicenseOwnerId { get; set; }

  [Required] public required string CompanyName { get; set; }
}