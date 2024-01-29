using FakeItEasy;
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
      Product = new ProductDetailsDto()
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

    var mediatorMock = A.Fake<IMediator>();
    var license = LicenseAggregate.Create(new LicenseKey("Some KEy"), createDto.LicenseOwner.ToLicenseOwner(),
      createDto.Product.ToLicensedProduct());
    A.CallTo(() => mediatorMock.Send(
        A<CreateLicenseCommand>.That.Matches(c =>
          c.LicenseOwner.LicenseOwnerId.ToString() == createDto.LicenseOwner.LicenseOwnerId &&
          c.LicenseOwner.CompanyName == createDto.LicenseOwner.CompanyName), A<CancellationToken>._))
      .Returns(Task.FromResult(license));

    var controller = new LicenseController(mediatorMock, NullLogger<LicenseController>.Instance);

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
      Product = new ProductDetailsDto()
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
    var mediatorMock = A.Fake<IMediator>();
    A.CallTo(() => mediatorMock.Send(
        A<CreateLicenseCommand>.That.Matches(c =>
          c.LicenseOwner.LicenseOwnerId.ToString() == createDto.LicenseOwner.LicenseOwnerId &&
          c.LicenseOwner.CompanyName == createDto.LicenseOwner.CompanyName), A<CancellationToken>._))
      .ThrowsAsync(new ValidationException(
        "Error",
        new ValidationFailure[] {new("someProp", "Some Message")}));

    var controller = new LicenseController(mediatorMock, NullLogger<LicenseController>.Instance);

    var result = await controller.Create(createDto);

    var res = (BadRequestResult) result;
    Assert.NotNull(res);
    res.StatusCode.Should().Be(400);
  }

  [Fact]
  public async void GetNotFound()
  {
    var licenseId = Guid.NewGuid();
    var mediatorMock = A.Fake<IMediator>();
    var controller = new LicenseController(mediatorMock, NullLogger<LicenseController>.Instance);
    LicenseDetailsDto? returnValue = null;
    A.CallTo(() => mediatorMock.Send(A<LicenseByIdQuery>._, A<CancellationToken>._))
      .Returns(Task.FromResult(returnValue));

    var result = await controller.GetLicense(licenseId);

    var notFoundResult = result as NotFoundResult;
    Assert.NotNull(notFoundResult);
  }

  [Fact]
  public async void GetFindsResult()
  {
    var licenseId = Guid.NewGuid();
    var mediatorMock = A.Fake<IMediator>();
    var controller = new LicenseController(mediatorMock, NullLogger<LicenseController>.Instance);
    var dataMock = new LicenseDetailsDto()
    {
      Id = licenseId.ToString(),
      LicenseKey = "Some Key",
      LicenseOwner = new LicenseOwnerDetailsDto()
      {
        CompanyName = "Some Company",
        LicenseOwnerId = "Some ID"
      },
      Product = new ProductDetailsDto()
      {
        ProductId = "454575",
        ProductName = "Super Solution",
        ProductAttributes = new ProductAttributesDto()
        {
          Flags = new Dictionary<string, bool>(),
          Options = new Dictionary<string, int>(),
          Configs = new Dictionary<string, string>()
        }
      },
      LicenseState = LicenseState.Active.ToString(),
      Version = 1
    };

    A.CallTo(() => mediatorMock.Send(
      A<LicenseByIdQuery>.That.Matches(
        q => q.Id == licenseId), A<CancellationToken>._))!.Returns(Task.FromResult(dataMock));

    var result = await controller.GetLicense(licenseId);

    var okObjectResult = result as OkObjectResult;
    Assert.NotNull(okObjectResult);
  }
}