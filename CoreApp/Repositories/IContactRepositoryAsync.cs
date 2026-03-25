using CoreApp.Dto;
using CoreApp.Entities;

namespace CoreApp.Repositories;

public interface IContactRepositoryAsync : IGenericRepositoryAsync<Contact>
{
    Task<PagedResult<Contact>> SearchAsync(ContactSearchDto searchDto);

    Task<IEnumerable<Contact>> FindByTagAsync(string tag);

    Task AddNoteAsync(Guid contactId, Note note);

    Task<IEnumerable<Note>> GetNotesAsync(Guid contactId);

    Task AddTagAsync(Guid contactId, string tag);

    Task RemoveTagAsync(Guid contactId, string tag);
}