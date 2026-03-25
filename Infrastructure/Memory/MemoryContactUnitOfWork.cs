using CoreApp.Repositories;

namespace Infrastructure.Memory;

public class MemoryContactUnitOfWork : IContactUnitOfWork
{
    private readonly IPersonRepositoryAsync _persons;
    private readonly ICompanyRepositoryAsync _companies;
    private readonly IOrganizationRepositoryAsync _organizations;
    private readonly IContactRepositoryAsync _contacts;

    public MemoryContactUnitOfWork(
        IPersonRepositoryAsync persons,
        ICompanyRepositoryAsync companies,
        IOrganizationRepositoryAsync organizations,
        IContactRepositoryAsync contacts)
    {
        _persons = persons;
        _companies = companies;
        _organizations = organizations;
        _contacts = contacts;
    }

    public IPersonRepositoryAsync Persons => _persons;
    public ICompanyRepositoryAsync Companies => _companies;
    public IOrganizationRepositoryAsync Organizations => _organizations;
    public IContactRepositoryAsync Contacts => _contacts;

    public ValueTask DisposeAsync()
    {
        return ValueTask.CompletedTask;
    }

    public Task<int> SaveChangesAsync()
    {
        return Task.FromResult(0);
    }

    public Task BeginTransactionAsync()
    {
        return Task.CompletedTask;
    }

    public Task CommitTransactionAsync()
    {
        return Task.CompletedTask;
    }

    public Task RollbackTransactionAsync()
    {
        return Task.CompletedTask;
    }
}