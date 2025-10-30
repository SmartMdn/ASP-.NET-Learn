using LibraryManagementAPI.DTOs;
using LibraryManagementAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LibraryQueriesController : ControllerBase
{
    private readonly ILibraryQueryService _queryService;

    public LibraryQueriesController(ILibraryQueryService queryService)
    {
        _queryService = queryService;
    }

    /// <summary>
    /// Получить всех авторов с количеством книг
    /// </summary>
    [HttpGet("authors-with-book-count")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AuthorWithBookCountDto>>> GetAuthorsWithBookCount()
    {
        var result = await _queryService.GetAuthorsWithBookCountAsync();
        return Ok(result);
    }

    /// <summary>
    /// Получить книги, опубликованные после указанного года
    /// </summary>
    [HttpGet("books-after-year/{year}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<BookDto>>> GetBooksPublishedAfterYear(int year)
    {
        if (year < 0 || year > DateTime.Now.Year)
            return BadRequest(new { message = "Некорректный год" });

        var result = await _queryService.GetBooksPublishedAfterYearAsync(year);
        return Ok(result);
    }

    /// <summary>
    /// Найти авторов по имени (поиск с Contains/StartsWith)
    /// </summary>
    [HttpGet("search-authors")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<AuthorDto>>> SearchAuthorsByName([FromQuery] string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return BadRequest(new { message = "Поисковый запрос не может быть пустым" });

        var result = await _queryService.SearchAuthorsByNameAsync(searchTerm);
        return Ok(result);
    }
}
