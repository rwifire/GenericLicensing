using FluentAssertions;
using GenericLicensing.Domain.Commands.License;
using GenericLicensing.Domain.Entities;
using GenericLicensing.Domain.Validators;
using GenericLicensing.Domain.ValueObjects;

namespace GenericLicensing.Domain.Tests.License.Commands;

public class CreateLicenseCommandTests
{
  [Fact]
  public void CreateLicenseCommandCreation()
  {
    var licenseOwner = new LicenseOwner(new LicenseOwnerId("123"), "Company");
    var prodAttributes = new ProductAttributes(new Dictionary<string, bool>(), new Dictionary<string, int>(),
      new Dictionary<string, string>());
    var licensedProduct = new LicensedProduct(new ProductId("222"), "Prod Name", prodAttributes);
    var validator = new CreateLicenseCommandValidator();

    var command = new CreateLicenseCommand(licenseOwner, licensedProduct, validator);

    command.LicenseOwner.Should().Be(licenseOwner);
    command.LicensedProduct.Should().Be(licensedProduct);
  }

  [Fact]
  public void CreateLicenseCommandLicenseOwnerValidation()
  {
    var licenseOwner = new LicenseOwner(new LicenseOwnerId("123"), "Company");
    var prodAttributes = new ProductAttributes(new Dictionary<string, bool>(), new Dictionary<string, int>(),
      new Dictionary<string, string>());
    var licensedProduct = new LicensedProduct(new ProductId("222"), "Prod Name", prodAttributes);
    var validator = new CreateLicenseCommandValidator();

    var command = new CreateLicenseCommand(licenseOwner, licensedProduct, validator);

    command.Validate().IsValid.Should().BeTrue();
  }

  [Fact]
  public void CreateLicenseCommandLicenseOwnerValidationFailsNullOwner()
  {
    LicenseOwner? licenseOwner = null;
    var prodAttributes = new ProductAttributes(new Dictionary<string, bool>(), new Dictionary<string, int>(),
      new Dictionary<string, string>());
    var licensedProduct = new LicensedProduct(new ProductId("222"), "Prod Name", prodAttributes);
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
    var licenseOwner = new LicenseOwner(new LicenseOwnerId("123"), "Company");
    LicensedProduct? licensedProduct = null;
    var validator = new CreateLicenseCommandValidator();
    var command = new CreateLicenseCommand(licenseOwner, licensedProduct!, validator);

    var result = command.Validate();

    result.IsValid.Should().BeFalse();
    result.Errors.Count.Should().Be(1);
    result.Errors[0].ErrorCode.Should().Be(CreateLicenseCommandValidator.PropertyRequiredCode);
    result.Errors[0].ErrorMessage.Should().Be(CreateLicenseCommandValidator.PropertyRequiredMessage);
    result.Errors[0].PropertyName.Should().Be(nameof(command.LicensedProduct));
  }
}