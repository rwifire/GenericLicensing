using DDDBase.Cqrs;
using FluentValidation;
using GenericLicensing.Domain.Aggregates;
using GenericLicensing.Domain.Commands.License;
using GenericLicensing.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GenericLicensing.Application.CommandHandler;

public class CreatelicenseCommandHandler : IRequestHandler<CreateLicenseCommand, LicenseAggregate>
{
  private readonly IEventService<LicenseAggregate> _eventService;
  private readonly ILogger<CreatelicenseCommandHandler> _logger;

  public CreatelicenseCommandHandler(IEventService<LicenseAggregate> eventService,
    ILogger<CreatelicenseCommandHandler> logger)
  {
    _eventService = eventService;
    _logger = logger;
  }

  public async Task<LicenseAggregate> Handle(CreateLicenseCommand request, CancellationToken cancellationToken)
  {
    var validationResult = request.Validate();
    if (!validationResult.IsValid)
    {
      throw new ValidationException(validationResult.Errors);
    }

    //TODO: LicenseKey generation should be responsibility of the License Aggregate
    var license =
      LicenseAggregate.Create(new LicenseKey(Guid.NewGuid().ToString()), request.LicenseOwner, request.Product);

    await _eventService.PersistAsync(license);

    return license;
  }
}