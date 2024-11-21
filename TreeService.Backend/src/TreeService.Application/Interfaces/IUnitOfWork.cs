using System.Data.Common;

namespace TreeService.Application.Interfaces;

public interface IUnitOfWork
{
    public Task<DbTransaction> BeginTransaction(CancellationToken cancellationToken = default);

    public Task SaveChanges(CancellationToken cancellationToken = default);
}
