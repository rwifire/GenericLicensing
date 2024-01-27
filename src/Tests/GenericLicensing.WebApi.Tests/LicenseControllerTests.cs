using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using GenericLicensing.Contracts.LicenseDtos;
using GenericLicensing.Domain.Aggregates;
using GenericLicensing.Domain.Commands.License;
using GenericLicensing.Domain.Entities;
using GenericLicensing.Domain.Queries;
using GenericLicensing.Domain.ValueObjects;
using GenericLicensing.WebApi.Controllers;
using GenericLicensing.WebApi.Mappers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace GenericLicensing.WebApi.Tests;

public class LicenseControllerTests
{
  [Fact]
  public async void CreateWithCorrectResult()
  {
    var createDto = new CreateLicenseDto()
    {
      LicenseOwner = new LicenseOwnerDetailsDto()
      {
        LicenseOwnerId = "123",
        CompanyName = "Some Company"
      },
      LicensedProduct = new LicensedProductDetailsDto()
      {
        ProductId = "222",
        ProductName = "Super Software",
        ProductAttributes = new ProductAttributesDto()
        {
          Flags = new Dictionary<string, bool>(),
          Options = new Dictionary<string, int>(),
          Configs = new Dictionary<string, string>()
        }
      }
    };

    var mediatorMock = new Mock<IMediator>();
    var license = LicenseAggregate.Create(new LicenseKey("Some KEy"), createDto.LicenseOwner.ToLicenseOwner(),
      createDto.LicensedProduct.ToLicensedProduct());
    mediatorMock.Setup(m =>
        m.Send(
          It.Is<CreateLicenseCommand>(c =>
            c.LicenseOwner.LicenseOwnerId.ToString() == createDto.LicenseOwner.LicenseOwnerId &&
            c.LicenseOwner.CompanyName == createDto.LicenseOwner.CompanyName),
          It.IsAny<CancellationToken>()))
      .ReturnsAsync(license);
    var controller = new LicenseController(mediatorMock.Object, NullLogger<LicenseController>.Instance);

    var result = await controller.Create(createDto);

    var res = (CreatedAtActionResult) result;
    Assert.NotNull(res);
    res.StatusCode.Should().Be(201);
    var createdObject = res.Value as LicenseDetailsDto;
    Assert.NotNull(createdObject);
    Assert.NotNull(res.RouteValues);
    res.RouteValues.ContainsKey("id").Should().BeTrue();
    res.RouteValues["id"].Should().Be(license.Id.ToString());
  }

  [Fact]
  public async void CreateWithValidationException()
  {
    var createDto = new CreateLicenseDto()
    {
      LicenseOwner = new LicenseOwnerDetailsDto()
      {
        LicenseOwnerId = "123",
        CompanyName = "Some Company"
      },
      LicensedProduct = new LicensedProductDetailsDto()
      {
        ProductId = "222",
        ProductName = "Super Software",
        ProductAttributes = new ProductAttributesDto()
        {
          Flags = new Dictionary<string, bool>(),
          Options = new Dictionary<string, int>(),
          Configs = new Dictionary<string, string>()
        }
      }
    };
    var mediatorMock = new Mock<IMediator>();
    var license = LicenseAggregate.Create(new LicenseKey("Some KEy"), createDto.LicenseOwner.ToLicenseOwner(),
      createDto.LicensedProduct.ToLicensedProduct());
    mediatorMock.Setup(m =>
        m.Send(
          It.Is<CreateLicenseCommand>(c =>
            c.LicenseOwner.LicenseOwnerId.ToString() == createDto.LicenseOwner.LicenseOwnerId &&
            c.LicenseOwner.CompanyName == createDto.LicenseOwner.CompanyName),
          It.IsAny<CancellationToken>()))
      .ThrowsAsync(new ValidationException(
        "Error",
        new ValidationFailure[] {new("someProp", "Some Message")}));

    var controller = new LicenseController(mediatorMock.Object, NullLogger<LicenseController>.Instance);

    var result = await controller.Create(createDto);

    var res = (BadRequestResult) result;
    Assert.NotNull(res);
    res.StatusCode.Should().Be(400);
  }

  [Fact]
  public async void GetNotFound()
  {
    var licenseId = Guid.NewGuid();
    var mediatorMock = new Mock<IMediator>();
    var controller = new LicenseController(mediatorMock.Object, NullLogger<LicenseController>.Instance);
    mediatorMock.Setup(m => m.Send(It.IsAny<LicenseByIdQuery>(), It.IsAny<CancellationToken>()))
      .ReturnsAsync(() => null);

    var result = await controller.GetLicense(licenseId);

    var notFoundResult = result as NotFoundResult;
    Assert.NotNull(notFoundResult);
  }

  [Fact]
  public async void GetFindsResult()
  {
    var licenseId = Guid.NewGuid();
    var mediatorMock = new Mock<IMediator>();
    var controller = new LicenseController(mediatorMock.Object, NullLogger<LicenseController>.Instance);
    var dataMock = new LicenseDetailsDto()
    {
      Id = licenseId.ToString(),
      LicenseKey = "Some Key",
      LicenseOwner = new LicenseOwnerDetailsDto()
      {
        CompanyName = "Some Company",
        LicenseOwnerId = "Some ID"
      },
      LicenseState = LicenseState.Active.ToString(),
      Version = 1
    };

    mediatorMock.Setup(m => m.Send(It.Is<LicenseByIdQuery>(
        q => q.Id == licenseId), It.IsAny<CancellationToken>()))
      .ReturnsAsync(dataMock);

    var result = await controller.GetLicense(licenseId);

    var okObjectResult = result as OkObjectResult;
    Assert.NotNull(okObjectResult);
  }
}