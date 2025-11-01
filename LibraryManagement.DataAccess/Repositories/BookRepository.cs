using LibraryManagement.DataAccess.Data;
using LibraryManagement.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.DataAccess.Repositories;

public class BookRepository(LibraryContext context) : IBookRepository
{
    public async Task<IEnumerable<Book>> GetAllAsync(int? publishedAfterYear = null)
    {
        var query = context.Books
            .Include(b => b.Author)
            .AsQueryable();
        
        if (publishedAfterYear.HasValue)
        {
            query = query.Where(b => b.PublishedYear > publishedAfterYear.Value);
        }
        
        return await query
            .OrderBy(b => b.PublishedYear)
            .ThenBy(b => b.Title)
            .ToListAsync();
    }

    public async Task<Book?> GetByIdAsync(int id)
    {
        return await context.Books
            .Include(b => b.Author)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<Book> CreateAsync(Book book)
    {
        context.Books.Add(book);
        await context.SaveChangesAsync();
        return book;
    }

    public async Task<bool> UpdateAsync(int id, Book book)
    {
        var existingBook = await context.Books.FindAsync(id);
        if (existingBook is null)
            return false;

        existingBook.Title = book.Title;
        existingBook.PublishedYear = book.PublishedYear;
        existingBook.AuthorId = book.AuthorId;
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var book = await context.Books.FindAsync(id);
        if (book is null)
            return false;

        context.Books.Remove(book);
        await context.SaveChangesAsync();
        return true;
    }
}
