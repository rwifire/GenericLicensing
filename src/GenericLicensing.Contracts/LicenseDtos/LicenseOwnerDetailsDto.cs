using System.ComponentModel.DataAnnotations;

namespace GenericLicensing.Contracts.LicenseDtos;

public class LicenseOwnerDetailsDto
{
  [Required] public string LicenseOwnerId { get; set; }

  [Required] public string CompanyName { get; set; }
}