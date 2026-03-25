using CoreApp.Entities;
using CoreApp.Repositories;
using CoreApp.Enums;

namespace Infrastructure.Memory;

public class MemoryPersonRepository 
    : MemoryGenericRepository<Person>, IPersonRepositoryAsync
{
    public MemoryPersonRepository() : base()
    {
        _data.Add(Guid.NewGuid(), new Person()
        {
            FirstName = "Adam",
            LastName = "Nowak",
            Gender = Gender.Male
        });

        _data.Add(Guid.NewGuid(), new Person()
        {
            FirstName = "Anna",
            LastName = "Kowalska",
            Gender = Gender.Female
        });
    }

    public Task<IEnumerable<Person>> FindByEmployerAsync(Guid companyId)
    {
        var result = _data.Values
            .Where(p => p.Employer != null && p.Employer.Id == companyId);

        return Task.FromResult(result);
    }

    public Task<IEnumerable<Person>> FindByOrganizationAsync(Guid organizationId)
    {
        var result = _data.Values
            .Where(p => p.Organization != null && p.Organization.Id == organizationId);

        return Task.FromResult(result);
    }
}