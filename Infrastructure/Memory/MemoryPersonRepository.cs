using CoreApp.Entities;
using CoreApp.Repositories;
using CoreApp.Enums;

namespace Infrastructure.Memory;

public class MemoryPersonRepository 
    : MemoryGenericRepository<Person>, IPersonRepositoryAsync
{
    public MemoryPersonRepository() : base()
    {
        var id1 = Guid.NewGuid();

        _data.Add(id1, new Person()
        {
            Id = id1,
            FirstName = "Adam",
            LastName = "Nowak",
            Gender = Gender.Male
        });

        var id2 = Guid.NewGuid();

        _data.Add(id2, new Person()
        {
            Id = id2,
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