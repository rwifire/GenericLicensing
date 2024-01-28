using FluentValidation;
using GenericLicensing.Contracts.LicenseDtos;
using GenericLicensing.Domain.Aggregates;
using GenericLicensing.Domain.Commands.License;
using GenericLicensing.Domain.Queries;
using GenericLicensing.Domain.Validators;
using GenericLicensing.WebApi.Mappers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GenericLicensing.WebApi.Controllers;

[ApiVersion("1.0")]
[Route("api/v{v:apiVersion}/[controller]")]
[ApiController]
public class LicenseController : ControllerBase
{
  private readonly ILogger<LicenseController> _logger;
  private readonly IMediator _mediator;

  public LicenseController(IMediator mediator, ILogger<LicenseController> logger)
  {
    _mediator = mediator;
    _logger = logger;
  }

  [HttpPost]
  [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(LicenseDetailsDto))]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public async Task<IActionResult> Create(CreateLicenseDto dto)
  {
    LicenseAggregate licenseAggregate;
    try
    {
      var command = new CreateLicenseCommand(dto.LicenseOwner.ToLicenseOwner(), dto.Product.ToLicensedProduct(),
        new CreateLicenseCommandValidator());
      licenseAggregate = await _mediator.Send(command);
    }
    catch (ValidationException ex)
    {
      foreach (var validationFailure in ex.Errors)
      {
        _logger.LogError("{ErrorCode}: {ErrorMessage} for property {Property}", validationFailure.ErrorCode,
          validationFailure.ErrorMessage, validationFailure.PropertyName);
      }

      return BadRequest();
    }

    return CreatedAtAction(nameof(GetLicense), new {id = licenseAggregate.Id.ToString()},
      licenseAggregate.ToLicenseDetails());
  }

  [HttpGet]
  [Route("{id:guid}", Name = "GetLicense")]
  [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LicenseDetailsDto))]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<IActionResult> GetLicense(Guid id)
  {
    var query = new LicenseByIdQuery(id);
    var result = await _mediator.Send(query);
    if (null == result)
    {
      return NotFound();
    }

    return new OkObjectResult(result);
  }
}