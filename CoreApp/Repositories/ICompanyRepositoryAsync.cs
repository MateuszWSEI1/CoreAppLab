using CoreApp.Entities;

namespace CoreApp.Repositories;

public interface ICompanyRepositoryAsync : IGenericRepositoryAsync<Company>
{
    Task<IEnumerable<Company>> FindByNameAsync(string name);

    Task<Company?> FindByNipAsync(string nip);

    Task<IEnumerable<Person>> GetEmployeesAsync(Guid companyId);
}