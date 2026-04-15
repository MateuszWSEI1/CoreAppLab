using CoreApp.Dto;
using CoreApp.Entities;

namespace CoreApp.Services;

public interface IPersonService
{
    Task<PagedResult<PersonDto>> FindAllPeoplePaged(int page, int size);

    Task<PersonDto?> FindByIdAsync(Guid id);

    Task<IAsyncEnumerable<PersonDto>> FindPeopleFromCompany(Guid companyId);

    Task<IAsyncEnumerable<PersonDto>> FindPeopleFromOrganization(Guid organizationId);

    Task<PersonDto> CreateAsync(CreatePersonDto dto);

    Task<PersonDto?> UpdateAsync(Guid id, UpdatePersonDto dto);

    Task<bool> DeleteAsync(Guid id);
    
    Task AddTagAsync(Guid personId, string tag);

    Task RemoveTagAsync(Guid personId, string tag);

    Task AddNoteAsync(Guid personId, string content);

    Task<IEnumerable<string>> GetNotesAsync(Guid personId);
    
    Task<Note> AddNoteToPerson(Guid personId, CreateNoteDto noteDto);
    
    Task<PersonDto> GetPerson(Guid personId);
    
}