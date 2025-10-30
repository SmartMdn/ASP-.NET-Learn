using LibraryManagementAPI.DTOs;

namespace LibraryManagementAPI.Services;

public interface ILibraryQueryService
{
    IEnumerable<AuthorWithBookCountDto> GetAuthorsWithBookCount();
    IEnumerable<BookDto> GetBooksPublishedAfterYear(int year);
    IEnumerable<AuthorDto> SearchAuthorsByName(string searchTerm);
}
