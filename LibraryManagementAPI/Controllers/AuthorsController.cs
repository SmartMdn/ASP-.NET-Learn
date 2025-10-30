using LibraryManagementAPI.DTOs;
using LibraryManagementAPI.Models;
using LibraryManagementAPI.Services;
using LibraryManagementAPI.Validators;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorsController : ControllerBase
{
    private readonly IAuthorService _authorService;

    public AuthorsController(IAuthorService authorService)
    {
        _authorService = authorService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Author>>> GetAll()
    {
        var authors = await _authorService.GetAllAsync();
        return Ok(authors);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Author>> GetById(int id)
    {
        var author = await _authorService.GetByIdAsync(id);
        if (author == null)
            return NotFound(new { message = $"Автор с ID {id} не найден" });

        return Ok(author);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Author>> Create([FromBody] CreateAuthorDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var validation = AuthorValidator.ValidateDateOfBirth(dto.DateOfBirth);
        if (!validation.IsValid)
            return BadRequest(new { message = validation.ErrorMessage });

        var author = new Author
        {
            Name = dto.Name,
            DateOfBirth = dto.DateOfBirth
        };

        var createdAuthor = await _authorService.CreateAsync(author);
        return CreatedAtAction(nameof(GetById), new { id = createdAuthor.Id }, createdAuthor);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateAuthorDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var validation = AuthorValidator.ValidateDateOfBirth(dto.DateOfBirth);
        if (!validation.IsValid)
            return BadRequest(new { message = validation.ErrorMessage });

        var author = new Author
        {
            Name = dto.Name,
            DateOfBirth = dto.DateOfBirth
        };

        var success = await _authorService.UpdateAsync(id, author);
        if (!success)
            return NotFound(new { message = $"Автор с ID {id} не найден" });

        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(int id)
    {
        var success = await _authorService.DeleteAsync(id);
        if (!success)
            return NotFound(new { message = $"Автор с ID {id} не найден" });

        return NoContent();
    }
}
