using LibraryManagement.BusinessLogic.DTOs;
using LibraryManagement.BusinessLogic.Services;
using LibraryManagementAPI.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController(IBookService bookService) : ControllerBase
{
    /// <summary>
    /// Получить все книги с опциональной фильтрацией
    /// </summary>
    /// <param name="publishedAfterYear">Фильтр: книги, опубликованные после указанного года</param>
    /// <response code="200">Список книг</response>
    /// <response code="400">Некорректный год</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<BookDto>>> GetAll([FromQuery] int? publishedAfterYear = null)
    {
        try
        {
            var books = await bookService.GetAllAsync(publishedAfterYear);
            return Ok(books);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    /// <summary>
    /// Получить книгу по ID
    /// </summary>
    /// <param name="id">ID книги</param>
    /// <response code="200">Книга найдена</response>
    /// <response code="404">Книга не найдена</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BookDto>> GetById(int id)
    {
        var book = await bookService.GetByIdAsync(id);
        if (book is null)
            return NotFound(new { message = $"Книга с ID {id} не найдена" });

        return Ok(book);
    }
    
    /// <summary>
    /// Создать новую книгу
    /// </summary>
    /// <param name="dto">Данные для создания книги</param>
    /// <response code="201">Книга успешно создана</response>
    /// <response code="400">Ошибка валидации или автор не найден</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BookDto>> Create([FromBody] CreateBookDto dto)
    {
        if (this.ValidateModelState() is { } badRequest)
            return badRequest;

        try
        {
            var createdBook = await bookService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdBook.Id }, createdBook);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    /// <summary>
    /// Обновить существующую книгу
    /// </summary>
    /// <param name="id">ID книги</param>
    /// <param name="dto">Новые данные книги</param>
    /// <response code="204">Книга успешно обновлена</response>
    /// <response code="400">Ошибка валидации или автор не найден</response>
    /// <response code="404">Книга не найдена</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateBookDto dto)
    {
        if (this.ValidateModelState() is { } badRequest)
            return badRequest;

        try
        {
            var success = await bookService.UpdateAsync(id, dto);
            if (!success)
                return NotFound(new { message = $"Книга с ID {id} не найдена" });

            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    /// <summary>
    /// Удалить книгу
    /// </summary>
    /// <param name="id">ID книги</param>
    /// <response code="204">Книга успешно удалена</response>
    /// <response code="404">Книга не найдена</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(int id)
    {
        var success = await bookService.DeleteAsync(id);
        if (!success)
            return NotFound(new { message = $"Книга с ID {id} не найдена" });

        return NoContent();
    }
}
