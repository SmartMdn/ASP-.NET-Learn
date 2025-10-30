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

    public IEnumerable<Author> GetAll()
    {
        // LINQ запрос: получить всех авторов с количеством книг
        return _context.Authors
            .Include(a => a.Books)
            .OrderBy(a => a.Name)
            .ToList();
    }

    public Author? GetById(int id)
    {
        // LINQ запрос: найти автора по ID с его книгами
        return _context.Authors
            .Include(a => a.Books)
            .FirstOrDefault(a => a.Id == id);
    }

    public Author Create(Author author)
    {
        _context.Authors.Add(author);
        _context.SaveChanges();
        return author;
    }

    public bool Update(int id, Author author)
    {
        var existingAuthor = _context.Authors.Find(id);
        if (existingAuthor == null)
            return false;

        existingAuthor.Name = author.Name;
        existingAuthor.DateOfBirth = author.DateOfBirth;
        _context.SaveChanges();
        return true;
    }

    public bool Delete(int id)
    {
        var author = _context.Authors.Find(id);
        if (author == null)
            return false;

        _context.Authors.Remove(author);
        _context.SaveChanges();
        return true;
    }
}
