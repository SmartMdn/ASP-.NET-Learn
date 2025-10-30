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

    public IEnumerable<Book> GetAll()
    {
        // LINQ запрос: получить все книги с информацией об авторах
        return _context.Books
            .Include(b => b.Author)
            .OrderBy(b => b.Title)
            .ToList();
    }

    public Book? GetById(int id)
    {
        // LINQ запрос: найти книгу по ID с информацией об авторе
        return _context.Books
            .Include(b => b.Author)
            .FirstOrDefault(b => b.Id == id);
    }

    public Book Create(Book book)
    {
        _context.Books.Add(book);
        _context.SaveChanges();
        return book;
    }

    public bool Update(int id, Book book)
    {
        var existingBook = _context.Books.Find(id);
        if (existingBook == null)
            return false;

        existingBook.Title = book.Title;
        existingBook.PublishedYear = book.PublishedYear;
        existingBook.AuthorId = book.AuthorId;
        _context.SaveChanges();
        return true;
    }

    public bool Delete(int id)
    {
        var book = _context.Books.Find(id);
        if (book == null)
            return false;

        _context.Books.Remove(book);
        _context.SaveChanges();
        return true;
    }
}
