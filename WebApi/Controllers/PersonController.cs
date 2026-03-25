using CoreApp.Dto;
using CoreApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonController : ControllerBase
{
    private readonly IPersonService _service;

    public PersonController(IPersonService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<PagedResult<PersonDto>> GetAll(int page = 1, int size = 10)
    {
        return await _service.FindAllPeoplePaged(page, size);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PersonDto>> GetById(Guid id)
    {
        var person = await _service.FindByIdAsync(id);

        if (person == null)
            return NotFound();

        return Ok(person);
    }

    [HttpPost]
    public async Task<ActionResult<PersonDto>> Create(CreatePersonDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return Ok(created);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _service.DeleteAsync(id);

        if (!result)
            return NotFound();

        return NoContent();
    }
}