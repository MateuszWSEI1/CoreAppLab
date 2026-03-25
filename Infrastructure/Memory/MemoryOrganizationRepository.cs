using CoreApp.Entities;
using CoreApp.Enums;
using CoreApp.Repositories;

namespace Infrastructure.Memory;

public class MemoryOrganizationRepository 
    : MemoryGenericRepository<Organization>, IOrganizationRepositoryAsync
{
    public Task<IEnumerable<Organization>> FindByTypeAsync(OrganizationType type)
    {
        var result = _data.Values
            .Where(o => o.Type == type);

        return Task.FromResult(result);
    }

    public Task<IEnumerable<Person>> GetMembersAsync(Guid organizationId)
    {
        var org = _data.Values.FirstOrDefault(o => o.Id == organizationId);

        if (org == null)
            throw new KeyNotFoundException();

        return Task.FromResult(org.Members.AsEnumerable());
    }
}