using LibraryManagement.DataAccess.Models;

namespace LibraryManagement.DataAccess.Repositories;

public interface IBookRepository
{
    Task<IEnumerable<Book>> GetAllAsync(int? publishedAfterYear = null);
    Task<Book?> GetByIdAsync(int id);
    Task<Book> CreateAsync(Book book);
    Task<bool> UpdateAsync(int id, Book book);
    Task<bool> DeleteAsync(int id);
}
