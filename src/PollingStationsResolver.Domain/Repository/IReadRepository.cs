using Ardalis.Specification;
using PollingStationsResolver.Domain.Entities;

namespace PollingStationsResolver.Domain.Repository;

public interface IReadRepository<T> : IReadRepositoryBase<T> where T : class, IAggregateRoot
{
}