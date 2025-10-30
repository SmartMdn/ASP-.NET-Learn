using LibraryManagementAPI.DTOs;

namespace LibraryManagementAPI.Services;

public interface ILibraryQueryService
{
    Task<IEnumerable<AuthorWithBookCountDto>> GetAuthorsWithBookCountAsync();
    Task<IEnumerable<BookDto>> GetBooksPublishedAfterYearAsync(int year);
    Task<IEnumerable<AuthorDto>> SearchAuthorsByNameAsync(string searchTerm);
}
