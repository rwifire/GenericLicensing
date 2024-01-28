using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GenericLicensing.IntegrationTests.Setup;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
  private ServiceProvider? _sp;

  protected override void ConfigureWebHost(IWebHostBuilder builder)
  {
    // builder.ConfigureServices(services =>
    // {
    //   var descriptor = services.SingleOrDefault(
    //     d => d.ServiceType ==
    //          typeof(DbContextOptions<LicenseDbContext>));
    //
    //   services.Remove(descriptor);
    //
    //   services.AddDbContext<LicenseDbContext>(options => { options.UseInMemoryDatabase("InMemoryDbForTesting"); });
    //
    //   _sp = services.BuildServiceProvider();
    //
    //   using (var scope = _sp.CreateScope())
    //   {
    //     var scopedServices = scope.ServiceProvider;
    //     var db = scopedServices.GetRequiredService<LicenseDbContext>();
    //     var logger = scopedServices
    //       .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();
    //
    //     db.Database.EnsureCreated();
    //   }
    // });
  }
}