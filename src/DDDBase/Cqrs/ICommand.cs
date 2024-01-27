using FluentValidation.Results;

namespace DDDBase.Cqrs;

public interface ICommand
{
  public ValidationResult Validate();
}