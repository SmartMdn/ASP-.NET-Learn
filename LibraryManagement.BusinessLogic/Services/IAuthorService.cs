using LibraryManagement.BusinessLogic.DTOs;

namespace LibraryManagement.BusinessLogic.Services;

public interface IAuthorService
{
    Task<IEnumerable<AuthorDto>> GetAllAsync(string? searchTerm = null);
    Task<IEnumerable<AuthorWithBookCountDto>> GetAuthorsWithBookCountAsync();
    Task<AuthorDto?> GetByIdAsync(int id);
    Task<AuthorDto> CreateAsync(CreateAuthorDto dto);
    Task<bool> UpdateAsync(int id, UpdateAuthorDto dto);
    Task<bool> DeleteAsync(int id);
}
