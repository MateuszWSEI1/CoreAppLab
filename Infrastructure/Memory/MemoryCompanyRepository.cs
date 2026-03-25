using CoreApp.Entities;
using CoreApp.Repositories;

namespace Infrastructure.Memory;

public class MemoryCompanyRepository 
    : MemoryGenericRepository<Company>, ICompanyRepositoryAsync
{
    public Task<IEnumerable<Company>> FindByNameAsync(string name)
    {
        var result = _data.Values
            .Where(c => c.Name != null && c.Name.Contains(name));

        return Task.FromResult(result);
    }

    public Task<Company?> FindByNipAsync(string nip)
    {
        var result = _data.Values
            .FirstOrDefault(c => c.NIP == nip);

        return Task.FromResult(result);
    }

    public Task<IEnumerable<Person>> GetEmployeesAsync(Guid companyId)
    {
        var company = _data.Values.FirstOrDefault(c => c.Id == companyId);

        if (company == null)
            throw new KeyNotFoundException();

        return Task.FromResult(company.Employees.AsEnumerable());
    }
}