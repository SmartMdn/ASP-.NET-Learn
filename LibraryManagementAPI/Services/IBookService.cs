using LibraryManagementAPI.Models;

namespace LibraryManagementAPI.Services;

public interface IBookService
{
    IEnumerable<Book> GetAll();
    Book? GetById(int id);
    Book Create(Book book);
    bool Update(int id, Book book);
    bool Delete(int id);
}
