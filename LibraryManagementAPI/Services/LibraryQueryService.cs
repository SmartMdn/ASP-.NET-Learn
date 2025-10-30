using LibraryManagementAPI.Data;
using LibraryManagementAPI.DTOs;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementAPI.Services;

public class LibraryQueryService : ILibraryQueryService
{
    private readonly LibraryContext _context;

    public LibraryQueryService(LibraryContext context)
    {
        _context = context;
    }

    // LINQ запрос: получить всех авторов с количеством книг
    public async Task<IEnumerable<AuthorWithBookCountDto>> GetAuthorsWithBookCountAsync()
    {
        return await _context.Authors
            .Select(a => new AuthorWithBookCountDto
            {
                Id = a.Id,
                Name = a.Name,
                DateOfBirth = a.DateOfBirth,
                BookCount = a.Books.Count
            })
            .OrderByDescending(a => a.BookCount)
            .ThenBy(a => a.Name)
            .ToListAsync();
    }

    // LINQ запрос: получить книги, опубликованные после указанного года
    public async Task<IEnumerable<BookDto>> GetBooksPublishedAfterYearAsync(int year)
    {
        return await _context.Books
            .Include(b => b.Author)
            .Where(b => b.PublishedYear > year)
            .OrderBy(b => b.PublishedYear)
            .ThenBy(b => b.Title)
            .Select(b => new BookDto
            {
                Id = b.Id,
                Title = b.Title,
                PublishedYear = b.PublishedYear,
                AuthorId = b.AuthorId,
                AuthorName = b.Author!.Name
            })
            .ToListAsync();
    }

    // LINQ запрос: найти автора по имени (с Contains или StartsWith)
    public async Task<IEnumerable<AuthorDto>> SearchAuthorsByNameAsync(string searchTerm)
    {
        return await _context.Authors
            .Where(a => a.Name.Contains(searchTerm) || a.Name.StartsWith(searchTerm))
            .OrderBy(a => a.Name)
            .Select(a => new AuthorDto
            {
                Id = a.Id,
                Name = a.Name,
                DateOfBirth = a.DateOfBirth
            })
            .ToListAsync();
    }
}
