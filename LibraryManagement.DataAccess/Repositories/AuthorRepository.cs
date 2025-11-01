using LibraryManagement.DataAccess.Data;
using LibraryManagement.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.DataAccess.Repositories;

public class AuthorRepository(LibraryContext context) : IAuthorRepository
{
    public async Task<IEnumerable<Author>> GetAllAsync(string? searchTerm = null)
    {
        var query = context.Authors
            .Include(a => a.Books)
            .AsQueryable();
        
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(a => a.Name.Contains(searchTerm));
        }
        
        return await query
            .OrderBy(a => a.Name)
            .ToListAsync();
    }

    public async Task<Author?> GetByIdAsync(int id)
    {
        return await context.Authors
            .Include(a => a.Books)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Author> CreateAsync(Author author)
    {
        context.Authors.Add(author);
        await context.SaveChangesAsync();
        return author;
    }

    public async Task<bool> UpdateAsync(int id, Author author)
    {
        var existingAuthor = await context.Authors.FindAsync(id);
        if (existingAuthor is null)
            return false;

        existingAuthor.Name = author.Name;
        existingAuthor.DateOfBirth = author.DateOfBirth;
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var author = await context.Authors.FindAsync(id);
        if (author is null)
            return false;

        context.Authors.Remove(author);
        await context.SaveChangesAsync();
        return true;
    }
    
    public async Task<IEnumerable<Author>> GetAuthorsWithBookCountAsync()
    {
        return await context.Authors
            .Include(a => a.Books)
            .OrderByDescending(a => a.Books.Count)
            .ThenBy(a => a.Name)
            .ToListAsync();
    }
}
