using CoreApp.Dto;
using CoreApp.Entities;
using CoreApp.Repositories;

namespace Infrastructure.Memory;

public class MemoryContactRepository 
    : MemoryGenericRepository<Contact>, IContactRepositoryAsync
{
    public Task<PagedResult<Contact>> SearchAsync(ContactSearchDto searchDto)
    {
        var query = _data.Values.AsQueryable();

        if (!string.IsNullOrEmpty(searchDto.Query))
        {
            query = query.Where(c =>
                (c.Mail != null && c.Mail.Contains(searchDto.Query)) ||
                (c.Phone != null && c.Phone.Contains(searchDto.Query)));
        }

        if (searchDto.Status != null)
        {
            query = query.Where(c => c.Status == searchDto.Status);
        }

        if (!string.IsNullOrEmpty(searchDto.Tag))
        {
            query = query.Where(c => c.Tags.Any(t => t.Name == searchDto.Tag));
        }

        var total = query.Count();

        var items = query
            .Skip((searchDto.Page - 1) * searchDto.PageSize)
            .Take(searchDto.PageSize)
            .ToList();

        var result = new PagedResult<Contact>(
            items,
            total,
            searchDto.Page,
            searchDto.PageSize
        );

        return Task.FromResult(result);
    }

    public Task<IEnumerable<Contact>> FindByTagAsync(string tag)
    {
        var result = _data.Values
            .Where(c => c.Tags.Any(t => t.Name == tag));

        return Task.FromResult(result);
    }

    public Task AddNoteAsync(Guid contactId, Note note)
    {
        if (!_data.ContainsKey(contactId))
            throw new KeyNotFoundException();

        _data[contactId].Notes.Add(note);

        return Task.CompletedTask;
    }

    public Task<IEnumerable<Note>> GetNotesAsync(Guid contactId)
    {
        if (!_data.ContainsKey(contactId))
            throw new KeyNotFoundException();

        return Task.FromResult(_data[contactId].Notes.AsEnumerable());
    }

    public Task AddTagAsync(Guid contactId, string tag)
    {
        if (!_data.ContainsKey(contactId))
            throw new KeyNotFoundException();

        var contact = _data[contactId];

        if (!contact.Tags.Any(t => t.Name == tag))
        {
            contact.Tags.Add(new Tag
            {
                Id = Guid.NewGuid(),
                Name = tag
            });
        }

        return Task.CompletedTask;
    }

    public Task RemoveTagAsync(Guid contactId, string tag)
    {
        if (!_data.ContainsKey(contactId))
            throw new KeyNotFoundException();

        var contact = _data[contactId];

        var tagToRemove = contact.Tags.FirstOrDefault(t => t.Name == tag);

        if (tagToRemove != null)
        {
            contact.Tags.Remove(tagToRemove);
        }

        return Task.CompletedTask;
    }
}