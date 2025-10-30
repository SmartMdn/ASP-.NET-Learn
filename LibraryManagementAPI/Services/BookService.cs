using LibraryManagementAPI.Data;
using LibraryManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementAPI.Services;

public class BookService : IBookService
{
    private readonly LibraryContext _context;

    public BookService(LibraryContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        // LINQ запрос: получить все книги с информацией об авторах
        return await _context.Books
            .Include(b => b.Author)
            .OrderBy(b => b.Title)
            .ToListAsync();
    }

    public async Task<Book?> GetByIdAsync(int id)
    {
        // LINQ запрос: найти книгу по ID с информацией об авторе
        return await _context.Books
            .Include(b => b.Author)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<Book> CreateAsync(Book book)
    {
        _context.Books.Add(book);
        await _context.SaveChangesAsync();
        return book;
    }

    public async Task<bool> UpdateAsync(int id, Book book)
    {
        var existingBook = await _context.Books.FindAsync(id);
        if (existingBook == null)
            return false;

        existingBook.Title = book.Title;
        existingBook.PublishedYear = book.PublishedYear;
        existingBook.AuthorId = book.AuthorId;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null)
            return false;

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();
        return true;
    }
}
