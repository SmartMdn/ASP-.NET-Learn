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
    public ActionResult<IEnumerable<AuthorWithBookCountDto>> GetAuthorsWithBookCount()
    {
        var result = _queryService.GetAuthorsWithBookCount();
        return Ok(result);
    }

    /// <summary>
    /// Получить книги, опубликованные после указанного года
    /// </summary>
    [HttpGet("books-after-year/{year}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<IEnumerable<BookDto>> GetBooksPublishedAfterYear(int year)
    {
        if (year < 0 || year > DateTime.Now.Year)
            return BadRequest(new { message = "Некорректный год" });

        var result = _queryService.GetBooksPublishedAfterYear(year);
        return Ok(result);
    }

    /// <summary>
    /// Найти авторов по имени (поиск с Contains/StartsWith)
    /// </summary>
    [HttpGet("search-authors")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<IEnumerable<AuthorDto>> SearchAuthorsByName([FromQuery] string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return BadRequest(new { message = "Поисковый запрос не может быть пустым" });

        var result = _queryService.SearchAuthorsByName(searchTerm);
        return Ok(result);
    }
}
