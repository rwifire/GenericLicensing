using FluentAssertions;
using GenericLicensing.Domain.Commands.License;
using GenericLicensing.Domain.Entities;
using GenericLicensing.Domain.Validators;
using GenericLicensing.Domain.ValueObjects;

namespace GenericLicensing.Domain.Tests.Commands;

public class CreateLicenseCommandTests
{
  [Fact]
  public void CreateLicenseCommandCreation()
  {
    var licenseOwner = new LicenseOwner(new LicenseOwnerId("123"), "Company");
    var validator = new CreateLicenseCommandValidator();

    var command = new CreateLicenseCommand(licenseOwner, validator);

    command.LicenseOwner.Should().Be(licenseOwner);
  }

  [Fact]
  public void CreateLicenseCommandLicenseOwnerValidation()
  {
    var licenseOwner = new LicenseOwner(new LicenseOwnerId("123"), "Company");
    var validator = new CreateLicenseCommandValidator();

    var command = new CreateLicenseCommand(licenseOwner, validator);

    command.Validate().IsValid.Should().BeTrue();
  }

  [Fact]
  public void CreateLicenseCommandLicenseOwnerValidationFails()
  {
    LicenseOwner? licenseOwner = null;
    var validator = new CreateLicenseCommandValidator();
    var command = new CreateLicenseCommand(licenseOwner!, validator);

    var result = command.Validate();

    result.IsValid.Should().BeFalse();
    result.Errors.Count.Should().Be(1);
    result.Errors[0].ErrorCode.Should().Be(CreateLicenseCommandValidator.PropertyRequiredCode);
    result.Errors[0].ErrorMessage.Should().Be(CreateLicenseCommandValidator.PropertyRequiredMessage);
  }
}