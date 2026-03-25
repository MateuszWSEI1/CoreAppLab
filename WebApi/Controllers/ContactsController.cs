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
}