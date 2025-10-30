using LibraryManagementAPI.Models;

namespace LibraryManagementAPI.Services;

public class AuthorService : IAuthorService
{
    private readonly List<Author> _authors = new();
    private readonly object _lock = new();
    private int _nextId = 1;

    public AuthorService()
    {
        SeedInitialData();
    }

    private void SeedInitialData()
    {
        _authors.Add(new Author { Id = _nextId++, Name = "Лев Толстой", DateOfBirth = new DateTime(1828, 9, 9) });
        _authors.Add(new Author { Id = _nextId++, Name = "Фёдор Достоевский", DateOfBirth = new DateTime(1821, 11, 11) });
        _authors.Add(new Author { Id = _nextId++, Name = "Антон Чехов", DateOfBirth = new DateTime(1860, 1, 29) });
    }

    public IEnumerable<Author> GetAll()
    {
        lock (_lock)
        {
            return _authors.ToList();
        }
    }

    public Author? GetById(int id)
    {
        lock (_lock)
        {
            return _authors.FirstOrDefault(a => a.Id == id);
        }
    }

    public Author Create(Author author)
    {
        lock (_lock)
        {
            author.Id = _nextId++;
            _authors.Add(author);
            return author;
        }
    }

    public bool Update(int id, Author author)
    {
        lock (_lock)
        {
            var existingAuthor = _authors.FirstOrDefault(a => a.Id == id);
            if (existingAuthor == null)
                return false;

            existingAuthor.Name = author.Name;
            existingAuthor.DateOfBirth = author.DateOfBirth;
            return true;
        }
    }

    public bool Delete(int id)
    {
        lock (_lock)
        {
            var author = _authors.FirstOrDefault(a => a.Id == id);
            if (author == null)
                return false;

            _authors.Remove(author);
            return true;
        }
    }
}
