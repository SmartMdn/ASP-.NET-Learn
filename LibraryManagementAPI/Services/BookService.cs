using LibraryManagementAPI.Models;

namespace LibraryManagementAPI.Services;

public class BookService : IBookService
{
    private readonly List<Book> _books = new();
    private readonly object _lock = new();
    private int _nextId = 1;

    public BookService()
    {
        SeedInitialData();
    }

    private void SeedInitialData()
    {
        _books.Add(new Book { Id = _nextId++, Title = "Война и мир", PublishedYear = 1869, AuthorId = 1 });
        _books.Add(new Book { Id = _nextId++, Title = "Анна Каренина", PublishedYear = 1877, AuthorId = 1 });
        _books.Add(new Book { Id = _nextId++, Title = "Преступление и наказание", PublishedYear = 1866, AuthorId = 2 });
        _books.Add(new Book { Id = _nextId++, Title = "Идиот", PublishedYear = 1869, AuthorId = 2 });
        _books.Add(new Book { Id = _nextId++, Title = "Вишнёвый сад", PublishedYear = 1904, AuthorId = 3 });
    }

    public IEnumerable<Book> GetAll()
    {
        lock (_lock)
        {
            return _books.ToList();
        }
    }

    public Book? GetById(int id)
    {
        lock (_lock)
        {
            return _books.FirstOrDefault(b => b.Id == id);
        }
    }

    public Book Create(Book book)
    {
        lock (_lock)
        {
            book.Id = _nextId++;
            _books.Add(book);
            return book;
        }
    }

    public bool Update(int id, Book book)
    {
        lock (_lock)
        {
            var existingBook = _books.FirstOrDefault(b => b.Id == id);
            if (existingBook == null)
                return false;

            existingBook.Title = book.Title;
            existingBook.PublishedYear = book.PublishedYear;
            existingBook.AuthorId = book.AuthorId;
            return true;
        }
    }

    public bool Delete(int id)
    {
        lock (_lock)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);
            if (book == null)
                return false;

            _books.Remove(book);
            return true;
        }
    }
}
