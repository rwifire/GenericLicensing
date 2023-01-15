using DDDBase.Cqrs;
using FluentValidation;
using FluentValidation.Results;
using GenericLicensing.Domain.Entities;
using MediatR;

namespace GenericLicensing.Domain.Commands.License;

public class CreateLicenseCommand : ICommand, IRequest<Aggregates.License>
{
  private readonly IValidator<CreateLicenseCommand> _validator;
  public LicenseOwner LicenseOwner { get; }

  public CreateLicenseCommand(LicenseOwner licenseOwner, IValidator<CreateLicenseCommand> validator)
  {
    _validator = validator;
    LicenseOwner = licenseOwner;
  }

  public ValidationResult Validate()
  {
    return _validator.Validate(this);
  }
}