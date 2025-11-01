using LibraryManagement.BusinessLogic.DTOs;
using LibraryManagement.BusinessLogic.Services;
using LibraryManagementAPI.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorsController(IAuthorService authorService) : ControllerBase
{
    /// <summary>
    /// Получить всех авторов с опциональным поиском
    /// </summary>
    /// <param name="searchTerm">Поисковый запрос по имени автора</param>
    /// <response code="200">Список авторов</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAll([FromQuery] string? searchTerm = null)
    {
        var authors = await authorService.GetAllAsync(searchTerm);
        return Ok(authors);
    }
    
    /// <summary>
    /// Получить авторов с количеством их книг
    /// </summary>
    /// <response code="200">Список авторов с количеством книг, отсортированный по убыванию количества</response>
    [HttpGet("with-book-count")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AuthorWithBookCountDto>>> GetAuthorsWithBookCount()
    {
        var authors = await authorService.GetAuthorsWithBookCountAsync();
        return Ok(authors);
    }

    /// <summary>
    /// Получить автора по ID
    /// </summary>
    /// <param name="id">ID автора</param>
    /// <response code="200">Автор найден</response>
    /// <response code="404">Автор не найден</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AuthorDto>> GetById(int id)
    {
        var author = await authorService.GetByIdAsync(id);
        if (author is null)
            return NotFound(new { message = $"Автор с ID {id} не найден" });

        return Ok(author);
    }

    /// <summary>
    /// Создать нового автора
    /// </summary>
    /// <param name="dto">Данные для создания автора</param>
    /// <response code="201">Автор успешно создан</response>
    /// <response code="400">Ошибка валидации (некорректная дата рождения)</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AuthorDto>> Create([FromBody] CreateAuthorDto dto)
    {
        if (this.ValidateModelState() is { } badRequest)
            return badRequest;

        try
        {
            var createdAuthor = await authorService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdAuthor.Id }, createdAuthor);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Обновить существующего автора
    /// </summary>
    /// <param name="id">ID автора</param>
    /// <param name="dto">Новые данные автора</param>
    /// <response code="204">Автор успешно обновлен</response>
    /// <response code="400">Ошибка валидации (некорректная дата рождения)</response>
    /// <response code="404">Автор не найден</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateAuthorDto dto)
    {
        if (this.ValidateModelState() is { } badRequest)
            return badRequest;

        try
        {
            var success = await authorService.UpdateAsync(id, dto);
            if (!success)
                return NotFound(new { message = $"Автор с ID {id} не найден" });

            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Удалить автора
    /// </summary>
    /// <param name="id">ID автора</param>
    /// <response code="204">Автор успешно удален</response>
    /// <response code="404">Автор не найден</response>
    /// <remarks>
    /// При удалении автора все его книги также будут удалены (Cascade Delete)
    /// </remarks>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(int id)
    {
        var success = await authorService.DeleteAsync(id);
        if (!success)
            return NotFound(new { message = $"Автор с ID {id} не найден" });

        return NoContent();
    }
}
