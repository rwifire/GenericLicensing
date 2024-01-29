using System.Text;
using System.Text.Json.Serialization;
using GenericLicensing.Contracts.LicenseDtos;
using GenericLicensing.IntegrationTests.Setup;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;

namespace GenericLicensing.IntegrationTests;

public class IntegrationTestFixture : IClassFixture<CustomWebApplicationFactory<Program>>
{
  protected readonly HttpClient _client;

  protected readonly CustomWebApplicationFactory<Program>
    _factory;

  public IntegrationTestFixture(
    CustomWebApplicationFactory<Program> factory)
  {
    _factory = factory;
    _client = factory.CreateClient(new WebApplicationFactoryClientOptions
    {
      AllowAutoRedirect = false
    });
  }
}