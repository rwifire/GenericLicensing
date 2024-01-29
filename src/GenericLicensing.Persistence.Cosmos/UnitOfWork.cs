using GenericLicensing.Contracts.Persistence;

namespace GenericLicensing.Persistence.Cosmos;

public class UnitOfWork : IDisposable, IUnitOfWork
{
  private readonly IGenericLicenseDbContext _context;

  public UnitOfWork(IGenericLicenseDbContext context)
  {
    _context = context;
  }

  public Task SaveChangesAsync(CancellationToken ct = default)
  {
    return _context.SaveChangesAsync(ct);
  }

  #region Dispose

  private bool _disposed = false;

  public void Dispose()
  {
    Dispose(true);
    GC.SuppressFinalize(this);
  }

  protected virtual void Dispose(bool disposing)
  {
    if (!_disposed)
    {
      if (disposing)
      {
        _context.DisposeAsync().AsTask().Wait();
      }
    }

    _disposed = true;
  }

  #endregion
}