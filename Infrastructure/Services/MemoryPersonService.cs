using CoreApp.Dto;
using CoreApp.Entities;
using CoreApp.Repositories;
using CoreApp.Services;

namespace Infrastructure.Services;

public class MemoryPersonService : IPersonService
{
    private readonly IContactUnitOfWork _unitOfWork;

    public MemoryPersonService(IContactUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<PagedResult<PersonDto>> FindAllPeoplePaged(int page, int size)
    {
        var people = await _unitOfWork.Persons.FindPagedAsync(page, size);

        var items = people.Items
            .Select(p => PersonDto.FromEntity(p))
            .ToList();

        return new PagedResult<PersonDto>(
            items,
            people.TotalCount,
            people.Page,
            people.PageSize
        );
    }
    
    public async Task<PersonDto?> FindByIdAsync(Guid id)
    {
        var person = await _unitOfWork.Persons.FindByIdAsync(id);

        return person == null ? null : PersonDto.FromEntity(person);
    }
    
    public async Task<IAsyncEnumerable<PersonDto>> FindPeopleFromCompany(Guid companyId)
    {
        var people = await _unitOfWork.Persons.FindByEmployerAsync(companyId);

        return ToAsyncEnumerable(people.Select(PersonDto.FromEntity));
    }
    
    public async Task<IAsyncEnumerable<PersonDto>> FindPeopleFromOrganization(Guid organizationId)
    {
        var people = await _unitOfWork.Persons.FindByOrganizationAsync(organizationId);

        return ToAsyncEnumerable(people.Select(PersonDto.FromEntity));
    }
    
    public async Task<PersonDto> CreateAsync(CreatePersonDto dto)
    {
        var entity = PersonDto.ToEntity(dto);

        var added = await _unitOfWork.Persons.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        return PersonDto.FromEntity(added);
    }
    
    public async Task<PersonDto?> UpdateAsync(Guid id, UpdatePersonDto dto)
    {
        var person = await _unitOfWork.Persons.FindByIdAsync(id);

        if (person == null)
            return null;
        
        if (dto.FirstName != null) person.FirstName = dto.FirstName;
        if (dto.LastName != null) person.LastName = dto.LastName;
        if (dto.Email != null) person.Mail = dto.Email;
        if (dto.Phone != null) person.Phone = dto.Phone;
        if (dto.Position != null) person.Position = dto.Position;
        if (dto.BirthDate != null) person.BirthDate = dto.BirthDate;
        if (dto.Gender != null) person.Gender = dto.Gender.Value;
        if (dto.Status != null) person.Status = dto.Status.Value;

        await _unitOfWork.Persons.UpdateAsync(person);
        await _unitOfWork.SaveChangesAsync();

        return PersonDto.FromEntity(person);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var existing = await _unitOfWork.Persons.FindByIdAsync(id);

        if (existing == null)
            return false;

        await _unitOfWork.Persons.RemoveByIdAsync(id);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task AddTagAsync(Guid personId, string tag)
    {
        await _unitOfWork.Contacts.AddTagAsync(personId, tag);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task RemoveTagAsync(Guid personId, string tag)
    {
        await _unitOfWork.Contacts.RemoveTagAsync(personId, tag);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task AddNoteAsync(Guid personId, string content)
    {
        var note = new Note
        {
            Id = Guid.NewGuid(),
            Content = content,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Contacts.AddNoteAsync(personId, note);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<string>> GetNotesAsync(Guid personId)
    {
        var notes = await _unitOfWork.Contacts.GetNotesAsync(personId);

        return notes.Select(n => n.Content);
    }

    private async IAsyncEnumerable<T> ToAsyncEnumerable<T>(IEnumerable<T> list)
    {
        foreach (var item in list)
        {
            yield return item;
            await Task.Yield();
        }
    }
}