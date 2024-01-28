using DDDBase.Cqrs;
using FluentValidation;
using FluentValidation.Results;
using GenericLicensing.Domain.Aggregates;
using GenericLicensing.Domain.Entities;
using GenericLicensing.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GenericLicensing.Domain.Commands.License;

public class CreateLicenseCommand : ICommand, IRequest<LicenseAggregate>
{
  private readonly IValidator<CreateLicenseCommand> _validator;
  public LicenseOwner LicenseOwner { get; }
  public Product Product { get; }

  public CreateLicenseCommand(LicenseOwner licenseOwner, Product product,
    IValidator<CreateLicenseCommand> validator)
  {
    _validator = validator;
    LicenseOwner = licenseOwner;
    Product = product;
  }

  public ValidationResult Validate()
  {
    return _validator.Validate(this);
  }
}

public class CreatelicenseCommandHandler : IRequestHandler<CreateLicenseCommand, LicenseAggregate>
{
  private readonly ILogger<CreatelicenseCommandHandler> _logger;

  public CreatelicenseCommandHandler(ILogger<CreatelicenseCommandHandler> logger)
  {
    _logger = logger;
  }

  public async Task<LicenseAggregate> Handle(CreateLicenseCommand request, CancellationToken cancellationToken)
  {
    var validationResult = request.Validate();
    if (!validationResult.IsValid)
    {
      throw new ValidationException(validationResult.Errors);
    }

    var license =
      LicenseAggregate.Create(new LicenseKey(Guid.NewGuid().ToString()), request.LicenseOwner, request.Product);

    return license;
  }
}