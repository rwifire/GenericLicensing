using GenericLicensing.Contracts.LicenseDtos;
using MediatR;

namespace GenericLicensing.Domain.Queries;

public class LicenseByIdQuery : IRequest<LicenseDetailsDto?>
{
  public Guid Id { get; }

  public LicenseByIdQuery(Guid id)
  {
    Id = id;
  }
}