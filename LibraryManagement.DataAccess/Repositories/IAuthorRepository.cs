using LibraryManagement.DataAccess.Models;

namespace LibraryManagement.DataAccess.Repositories;

public interface IAuthorRepository
{
    Task<IEnumerable<Author>> GetAllAsync(string? searchTerm = null);
    Task<Author?> GetByIdAsync(int id);
    Task<Author> CreateAsync(Author author);
    Task<bool> UpdateAsync(int id, Author author);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<Author>> GetAuthorsWithBookCountAsync();
}
