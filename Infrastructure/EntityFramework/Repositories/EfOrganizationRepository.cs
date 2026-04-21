using CoreApp.Entities;
using CoreApp.Enums;
using CoreApp.Repositories;
using Infrastructure.EntityFramework.Context;

namespace Infrastructure.EntityFramework.Repositories;

public class EfOrganizationRepository(ContactsDbContext context)
    : EfGenericRepository<Organization>(context.Organizations),
        IOrganizationRepositoryAsync
{
    public Task<IEnumerable<Organization>> FindByTypeAsync(OrganizationType type)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Person>> GetMembersAsync(Guid organizationId)
    {
        throw new NotImplementedException();
    }
}