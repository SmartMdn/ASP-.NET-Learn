using LibraryManagement.BusinessLogic.DTOs;

namespace LibraryManagement.BusinessLogic.Services;

public interface IBookService
{
    Task<IEnumerable<BookDto>> GetAllAsync(int? publishedAfterYear = null);
    Task<BookDto?> GetByIdAsync(int id);
    Task<BookDto> CreateAsync(CreateBookDto dto);
    Task<bool> UpdateAsync(int id, UpdateBookDto dto);
    Task<bool> DeleteAsync(int id);
}
