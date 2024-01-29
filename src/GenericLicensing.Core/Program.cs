using System.Net.Mime;
using GenericLicensing.Application.Behaviors;
using GenericLicensing.Application.CommandHandler;
using GenericLicensing.Core;
using GenericLicensing.Domain.Commands.License;
using GenericLicensing.Domain.Events.License;
using GenericLicensing.Persistence.Cosmos.EventStore;
using MediatR;
using Serilog;

Log.Logger = new LoggerConfiguration()
  .MinimumLevel.Information()
  .Enrich.FromLogContext()
  .MinimumLevel.Information()
  .WriteTo.Console(
    outputTemplate: "[{Timestamp:HH:mm:ss} {Level} {CorrelationId}] {Message}{NewLine}{Exception}")
  .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));
builder.Services.AddControllers();
builder.Services.AddApiVersioning();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddMediatR(typeof(CreateLicenseCommand), typeof(CreatelicenseCommandHandler));
builder.Services.AddMediatR(config =>
{
  config.RegisterServicesFromAssembly(typeof(CreateLicenseCommand).Assembly);
  config.RegisterServicesFromAssembly(typeof(CreatelicenseCommandHandler).Assembly);
  config.AddOpenBehavior(typeof(UnitOfWorkBehavior<,>));
});

builder.Services.AddScoped<IEventSerializer>(s => new EventSerializer(new[]
{
  typeof(LicenseCreatedEvent).Assembly
}));

builder.Services.AddCosmosDbInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program
{
}