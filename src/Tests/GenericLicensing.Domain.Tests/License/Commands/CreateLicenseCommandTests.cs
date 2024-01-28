using FluentAssertions;
using GenericLicensing.Domain.Commands.License;
using GenericLicensing.Domain.Entities;
using GenericLicensing.Domain.Validators;
using GenericLicensing.Domain.ValueObjects;
using GenericLicensing.TestUtils;

namespace GenericLicensing.Domain.Tests.License.Commands;

public class CreateLicenseCommandTests
{
  [Fact]
  public void CreateLicenseCommandCreation()
  {
    var licenseOwner = LicenseOwnerTestModel.CreateLicenseOwner();
    var licensedProduct = ProductTestModel.CreateProduct();
    var validator = new CreateLicenseCommandValidator();

    var command = new CreateLicenseCommand(licenseOwner, licensedProduct, validator);

    command.LicenseOwner.Should().Be(licenseOwner);
    command.Product.Should().Be(licensedProduct);
  }

  [Fact]
  public void CreateLicenseCommandLicenseOwnerValidation()
  {
    var licenseOwner = LicenseOwnerTestModel.CreateLicenseOwner();
    var licensedProduct = ProductTestModel.CreateProduct();
    var validator = new CreateLicenseCommandValidator();

    var command = new CreateLicenseCommand(licenseOwner, licensedProduct, validator);

    command.Validate().IsValid.Should().BeTrue();
  }

  [Fact]
  public void CreateLicenseCommandLicenseOwnerValidationFailsNullOwner()
  {
    LicenseOwner? licenseOwner = null;
    var licensedProduct = ProductTestModel.CreateProduct();
    var validator = new CreateLicenseCommandValidator();
    var command = new CreateLicenseCommand(licenseOwner!, licensedProduct, validator);

    var result = command.Validate();

    result.IsValid.Should().BeFalse();
    result.Errors.Count.Should().Be(1);
    result.Errors[0].ErrorCode.Should().Be(CreateLicenseCommandValidator.PropertyRequiredCode);
    result.Errors[0].ErrorMessage.Should().Be(CreateLicenseCommandValidator.PropertyRequiredMessage);
    result.Errors[0].PropertyName.Should().Be(nameof(command.LicenseOwner));
  }

  [Fact]
  public void CreateLicenseCommandLicenseOwnerValidationFailsNullProduct()
  {
    var licenseOwner = LicenseOwnerTestModel.CreateLicenseOwner();
    Product? licensedProduct = null;
    var validator = new CreateLicenseCommandValidator();
    var command = new CreateLicenseCommand(licenseOwner, licensedProduct!, validator);

    var result = command.Validate();

    result.IsValid.Should().BeFalse();
    result.Errors.Count.Should().Be(1);
    result.Errors[0].ErrorCode.Should().Be(CreateLicenseCommandValidator.PropertyRequiredCode);
    result.Errors[0].ErrorMessage.Should().Be(CreateLicenseCommandValidator.PropertyRequiredMessage);
    result.Errors[0].PropertyName.Should().Be(nameof(command.Product));
  }
}