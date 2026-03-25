using CoreApp.Repositories;

namespace CoreApp.Repositories;

public interface IContactUnitOfWork : IAsyncDisposable
{
    IPersonRepositoryAsync Persons { get; }
    ICompanyRepositoryAsync Companies { get; }
    IOrganizationRepositoryAsync Organizations { get; }
    IContactRepositoryAsync Contacts { get; }
    

    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}