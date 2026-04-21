using CoreApp.Dto;
using CoreApp.Entities;
using CoreApp.Repositories;
using Infrastructure.EntityFramework.Context;

namespace Infrastructure.EntityFramework.Repositories;

public class EfContactRepository(ContactsDbContext context)
    : EfGenericRepository<Contact>(context.Set<Contact>()),
        IContactRepositoryAsync
{
    public Task<PagedResult<Contact>> SearchAsync(ContactSearchDto searchDto)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Contact>> FindByTagAsync(string tag)
    {
        throw new NotImplementedException();
    }

    public Task AddNoteAsync(Guid contactId, Note note)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Note>> GetNotesAsync(Guid contactId)
    {
        throw new NotImplementedException();
    }

    public Task AddTagAsync(Guid contactId, string tag)
    {
        throw new NotImplementedException();
    }

    public Task RemoveTagAsync(Guid contactId, string tag)
    {
        throw new NotImplementedException();
    }
}