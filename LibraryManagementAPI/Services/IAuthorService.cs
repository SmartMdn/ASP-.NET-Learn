using LibraryManagementAPI.Models;

namespace LibraryManagementAPI.Services;

public interface IAuthorService
{
    IEnumerable<Author> GetAll();
    Author? GetById(int id);
    Author Create(Author author);
    bool Update(int id, Author author);
    bool Delete(int id);
}
