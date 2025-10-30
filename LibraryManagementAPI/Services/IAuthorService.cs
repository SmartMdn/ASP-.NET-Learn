using LibraryManagementAPI.Models;

namespace LibraryManagementAPI.Services;

public interface IAuthorService
{
    Task<IEnumerable<Author>> GetAllAsync();
    Task<Author?> GetByIdAsync(int id);
    Task<Author> CreateAsync(Author author);
    Task<bool> UpdateAsync(int id, Author author);
    Task<bool> DeleteAsync(int id);
}
