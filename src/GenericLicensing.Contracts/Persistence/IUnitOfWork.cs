namespace GenericLicensing.Contracts.Persistence;

public interface IUnitOfWork
{
  Task SaveChangesAsync(CancellationToken ct = default);

  void Dispose();
}