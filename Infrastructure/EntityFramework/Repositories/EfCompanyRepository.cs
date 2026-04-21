using CoreApp.Entities;
using CoreApp.Repositories;
using Infrastructure.EntityFramework.Context;

namespace Infrastructure.EntityFramework.Repositories;

public class EfCompanyRepository(ContactsDbContext context)
    : EfGenericRepository<Company>(context.Companies),
        ICompanyRepositoryAsync
{
    public Task<Company?> FindByNipAsync(string nip)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Company>> FindByNameAsync(string name)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Person>> GetEmployeesAsync(Guid companyId)
    {
        throw new NotImplementedException();
    }
}