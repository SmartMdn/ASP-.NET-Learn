using LibraryManagement.BusinessLogic.DTOs;
using LibraryManagement.BusinessLogic.Validators;
using LibraryManagement.DataAccess.Models;
using LibraryManagement.DataAccess.Repositories;

namespace LibraryManagement.BusinessLogic.Services;

public class AuthorService(IAuthorRepository repository) : IAuthorService
{
    public async Task<IEnumerable<AuthorDto>> GetAllAsync(string? searchTerm = null)
    {
        var authors = await repository.GetAllAsync(searchTerm);
        return authors.Select(MapToDto);
    }

    public async Task<IEnumerable<AuthorWithBookCountDto>> GetAuthorsWithBookCountAsync()
    {
        var authors = await repository.GetAuthorsWithBookCountAsync();
        return authors.Select(a => new AuthorWithBookCountDto
        {
            Id = a.Id,
            Name = a.Name,
            DateOfBirth = a.DateOfBirth,
            BookCount = a.Books.Count
        });
    }

    public async Task<AuthorDto?> GetByIdAsync(int id)
    {
        var author = await repository.GetByIdAsync(id);
        return author is null ? null : MapToDto(author);
    }

    public async Task<AuthorDto> CreateAsync(CreateAuthorDto dto)
    {
        var validation = AuthorValidator.ValidateDateOfBirth(dto.DateOfBirth);
        if (!validation.IsValid)
            throw new ArgumentException(validation.ErrorMessage);

        var author = new Author
        {
            Name = dto.Name,
            DateOfBirth = dto.DateOfBirth
        };

        var createdAuthor = await repository.CreateAsync(author);
        return MapToDto(createdAuthor);
    }

    public async Task<bool> UpdateAsync(int id, UpdateAuthorDto dto)
    {
        var validation = AuthorValidator.ValidateDateOfBirth(dto.DateOfBirth);
        if (!validation.IsValid)
            throw new ArgumentException(validation.ErrorMessage);

        var author = new Author
        {
            Name = dto.Name,
            DateOfBirth = dto.DateOfBirth
        };

        return await repository.UpdateAsync(id, author);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await repository.DeleteAsync(id);
    }

    private static AuthorDto MapToDto(Author author)
    {
        return new AuthorDto
        {
            Id = author.Id,
            Name = author.Name,
            DateOfBirth = author.DateOfBirth
        };
    }
}
