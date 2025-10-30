using LibraryManagementAPI.Data;
using LibraryManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementAPI.Services;

public class AuthorService : IAuthorService
{
    private readonly LibraryContext _context;

    public AuthorService(LibraryContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Author>> GetAllAsync()
    {
        // LINQ запрос: получить всех авторов с количеством книг
        return await _context.Authors
            .Include(a => a.Books)
            .OrderBy(a => a.Name)
            .ToListAsync();
    }

    public async Task<Author?> GetByIdAsync(int id)
    {
        // LINQ запрос: найти автора по ID с его книгами
        return await _context.Authors
            .Include(a => a.Books)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Author> CreateAsync(Author author)
    {
        _context.Authors.Add(author);
        await _context.SaveChangesAsync();
        return author;
    }

    public async Task<bool> UpdateAsync(int id, Author author)
    {
        var existingAuthor = await _context.Authors.FindAsync(id);
        if (existingAuthor == null)
            return false;

        existingAuthor.Name = author.Name;
        existingAuthor.DateOfBirth = author.DateOfBirth;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var author = await _context.Authors.FindAsync(id);
        if (author == null)
            return false;

        _context.Authors.Remove(author);
        await _context.SaveChangesAsync();
        return true;
    }
}
