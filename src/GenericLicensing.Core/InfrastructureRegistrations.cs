using DDDBase.Cqrs;
using DDDBase.Models;
using GenericLicensing.Application.Eventing;
using GenericLicensing.Domain.Aggregates;
using GenericLicensing.Persistence.Cosmos;
using GenericLicensing.Persistence.Cosmos.EventStore;
using Microsoft.EntityFrameworkCore;

namespace GenericLicensing.Core;

public static class InfrastructureRegistrations
{
  public static IServiceCollection AddCosmosDbInfrastructure(this IServiceCollection services,
    IConfiguration config)
  {
    services.AddDbContext<GenericLicenseDbContext>(options =>
      options.UseCosmos(config["cosmosEndpoint"], config["cosmosSecret"], config["dbName"]));
    services.AddScoped<IGenericLicenseDbContext, GenericLicenseDbContext>();

    services.AddAggregateInfrastructure<LicenseAggregate>(config);

    return services;
  }

  private static void AddAggregateInfrastructure<TA>(this IServiceCollection services, IConfiguration config)
    where TA : class, IAggregateRoot
  {
    services.AddScoped<IEventRepository<TA>, EventRepository<TA>>();
    services.AddScoped<IEventService<TA>, EventService<TA>>();
  }
}