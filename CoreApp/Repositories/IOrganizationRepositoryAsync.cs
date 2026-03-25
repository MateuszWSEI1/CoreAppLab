using CoreApp.Entities;
using CoreApp.Enums;

namespace CoreApp.Repositories;

public interface IOrganizationRepositoryAsync : IGenericRepositoryAsync<Organization>
{
    Task<IEnumerable<Organization>> FindByTypeAsync(OrganizationType type);

    Task<IEnumerable<Person>> GetMembersAsync(Guid organizationId);
}