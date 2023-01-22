using FluentValidation;
using GenericLicensing.Domain.Commands.License;

namespace GenericLicensing.Domain.Validators;

public class CreateLicenseCommandValidator : AbstractValidator<CreateLicenseCommand>
{
  public const string PropertyRequiredMessage = "This property is required to be present and not null!";
  public const string PropertyRequiredCode = "PropReq";

  public CreateLicenseCommandValidator()
  {
    RuleFor(x => x.LicenseOwner).NotNull().WithMessage(PropertyRequiredMessage).WithErrorCode(PropertyRequiredCode);
    RuleFor(x => x.LicensedProduct).NotNull().WithMessage(PropertyRequiredMessage).WithErrorCode(PropertyRequiredCode);
  }
}