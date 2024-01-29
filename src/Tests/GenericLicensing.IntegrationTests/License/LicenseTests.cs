using System.Diagnostics;
using System.Text;
using FluentAssertions;
using GenericLicensing.Contracts.LicenseDtos;
using GenericLicensing.IntegrationTests.Setup;
using Newtonsoft.Json;
using Xunit;

namespace GenericLicensing.IntegrationTests.License;

public class LicenseTests : IntegrationTestFixture
{
  public LicenseTests(CustomWebApplicationFactory<Program> factory) : base(factory)
  {
  }

  [Fact]
  public async Task Post_Create_ReturnsCorrectDto()
  {
    var createDto = new CreateLicenseDto()
    {
      LicenseOwner = new LicenseOwnerDetailsDto()
      {
        CompanyName = "My Test Company",
        LicenseOwnerId = "001245"
      },
      Product = new ProductDetailsDto()
      {
        ProductId = "P0154",
        ProductName = "Super Tool",
        ProductAttributes = new ProductAttributesDto()
        {
          Flags = new Dictionary<string, bool>()
          {
            {"SampleFlag", true}
          },
          Options = new Dictionary<string, int>()
          {
            {"SampleOption", 12}
          },
          Configs = new Dictionary<string, string>()
          {
            {"Sample Config", "BasUri"}
          }
        }
      }
    };

    HttpContent stringContent =
      new StringContent(JsonConvert.SerializeObject(createDto), Encoding.UTF8, "application/json");
    var response = await _client.PostAsync(Constants.LicenseUrl, stringContent);
    var content = await response.Content.ReadAsStringAsync();
    var responseDto = JsonConvert.DeserializeObject<LicenseDetailsDto>(content);

    responseDto.Should().NotBeNull();
    responseDto!.LicenseKey.Should().NotBeNullOrWhiteSpace();
    responseDto.LicenseOwner.Should().BeEquivalentTo(createDto.LicenseOwner);
    responseDto.Product.Should().BeEquivalentTo(createDto.Product);
  }
}