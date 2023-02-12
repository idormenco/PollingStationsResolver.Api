using Ardalis.Specification.EntityFrameworkCore;
using PollingStationsResolver.Domain.Entities;

namespace PollingStationsResolver.Domain.Repository;

public class EfRepository<T> : RepositoryBase<T>, IReadRepository<T>, IRepository<T> where T : class, IAggregateRoot
{
    public EfRepository(PollingStationsResolverContext dbContext) : base(dbContext)
    {
    }
}