using LibraryManagementAPI.DTOs;
using LibraryManagementAPI.Models;
using LibraryManagementAPI.Services;
using LibraryManagementAPI.Validators;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;
    private readonly IAuthorService _authorService;

    public BooksController(IBookService bookService, IAuthorService authorService)
    {
        _bookService = bookService;
        _authorService = authorService;
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Book>>> GetAll()
    {
        var books = await _bookService.GetAllAsync();
        return Ok(books);
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Book>> GetById(int id)
    {
        var book = await _bookService.GetByIdAsync(id);
        if (book == null)
            return NotFound(new { message = $"Книга с ID {id} не найдена" });

        return Ok(book);
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Book>> Create([FromBody] CreateBookDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var yearValidation = BookValidator.ValidatePublishedYear(dto.PublishedYear);
        if (!yearValidation.IsValid)
            return BadRequest(new { message = yearValidation.ErrorMessage });

        var author = await _authorService.GetByIdAsync(dto.AuthorId);
        if (author == null)
            return BadRequest(new { message = $"Автор с ID {dto.AuthorId} не найден" });

        var book = new Book
        {
            Title = dto.Title,
            PublishedYear = dto.PublishedYear,
            AuthorId = dto.AuthorId
        };

        var createdBook = await _bookService.CreateAsync(book);
        return CreatedAtAction(nameof(GetById), new { id = createdBook.Id }, createdBook);
    }
    
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateBookDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var yearValidation = BookValidator.ValidatePublishedYear(dto.PublishedYear);
        if (!yearValidation.IsValid)
            return BadRequest(new { message = yearValidation.ErrorMessage });

        var author = await _authorService.GetByIdAsync(dto.AuthorId);
        if (author == null)
            return BadRequest(new { message = $"Автор с ID {dto.AuthorId} не найден" });

        var book = new Book
        {
            Title = dto.Title,
            PublishedYear = dto.PublishedYear,
            AuthorId = dto.AuthorId
        };

        var success = await _bookService.UpdateAsync(id, book);
        if (!success)
            return NotFound(new { message = $"Книга с ID {id} не найдена" });

        return NoContent();
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(int id)
    {
        var success = await _bookService.DeleteAsync(id);
        if (!success)
            return NotFound(new { message = $"Книга с ID {id} не найдена" });

        return NoContent();
    }
}
