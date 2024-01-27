namespace DDDBase.Models;

public interface IEntity
{
  /// <summary>
  /// Id of the entity
  /// </summary>
  Guid Id { get; }
}