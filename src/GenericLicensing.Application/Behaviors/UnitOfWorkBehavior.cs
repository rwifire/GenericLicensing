using DDDBase.Cqrs;
using GenericLicensing.Persistence.Cosmos;
using MediatR;

namespace GenericLicensing.Application.Behaviors;

public sealed class UnitOfWorkBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
  where TRequest : IRequest<TResponse>
{
  private readonly IUnitOfWork _unitOfWork;

  public UnitOfWorkBehavior(IUnitOfWork unitOfWork)
  {
    _unitOfWork = unitOfWork;
  }

  public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
    CancellationToken cancellationToken)
  {
    if (request is not ICommand)
    {
      return await next();
    }

    var response = await next();
    await _unitOfWork.SaveChangesAsync(cancellationToken);
    return response;
  }
}