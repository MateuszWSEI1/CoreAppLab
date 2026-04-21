using CoreApp.Dto;
using CoreApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/contacts")]
public class ContactsController : ControllerBase
{
    private readonly IPersonService _service;

    public ContactsController(IPersonService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPersons([FromQuery] int page = 1, [FromQuery] int size = 10)
    {
        var result = await _service.FindAllPeoplePaged(page, size);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetPerson(Guid id)
    {
        var dto = await _service.FindByIdAsync(id);

        if (dto == null)
        {
            return NotFound();
        }

        return Ok(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreatePersonDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetPerson), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdatePersonDto dto)
    {
        var existingPerson = await _service.FindByIdAsync(id);
        
        if (existingPerson == null)
        {
            return NotFound();
        }

        var result = await _service.UpdateAsync(id, dto);
        return Ok(result);
    }
    [HttpPost("{contactId:guid}/notes")]
    [ProducesResponseType(typeof(NoteDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddNote(
        [FromRoute] Guid contactId,
        [FromBody] CreateNoteDto dto)
    {
        var note = await _service.AddNoteToPerson(contactId, dto);
        return CreatedAtAction(
            nameof(GetNotes),
            new { contactId },
            note);
    }

    [HttpGet("{contactId:guid}/notes")]
    [ProducesResponseType(typeof(IEnumerable<NoteDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetNotes([FromRoute] Guid contactId)
    {
        var person = await _service.GetPerson(contactId);
        return Ok(person.Notes);
    }
    [HttpDelete("{contactId:guid}/notes/{noteId:guid}")]
    public async Task<IActionResult> DeleteNote(
        [FromRoute] Guid contactId,
        [FromRoute] Guid noteId)
    {
        await _service.DeleteNote(contactId, noteId);
        return NoContent();
    }
}