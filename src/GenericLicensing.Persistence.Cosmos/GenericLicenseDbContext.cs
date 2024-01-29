using GenericLicensing.Persistence.Cosmos.EventStore;
using Microsoft.EntityFrameworkCore;

namespace GenericLicensing.Persistence.Cosmos;

public class GenericLicenseDbContext : DbContext, IGenericLicenseDbContext
{
  public GenericLicenseDbContext(DbContextOptions<GenericLicenseDbContext> options) : base(options)
  {
    Database.EnsureCreatedAsync().Wait();
  }

  public DbSet<EventData> Events { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<EventData>()
      .ToContainer("EventsSource");
    modelBuilder.Entity<EventData>()
      .HasNoDiscriminator();
    modelBuilder.Entity<EventData>()
      .HasPartitionKey(o => o.AggregateId);
    modelBuilder.Entity<EventData>()
      .HasKey(o => o.Id);
    modelBuilder.Entity<EventData>()
      .Property(o => o.AggregateId)
      .HasConversion(v => v.ToString(), v => Guid.Parse(v));
  }
}

public interface IGenericLicenseDbContext
{
  DbSet<EventData> Events { get; }

  Task<int> SaveChangesAsync(CancellationToken ct = default);

  ValueTask DisposeAsync();
}