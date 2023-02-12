using Ardalis.Specification;
using PollingStationsResolver.Domain.Entities;

namespace PollingStationsResolver.Domain.Repository;

public interface IRepository<T> : IRepositoryBase<T> where T : class, IAggregateRoot
{
}