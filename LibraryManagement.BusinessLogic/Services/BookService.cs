using LibraryManagement.BusinessLogic.DTOs;
using LibraryManagement.BusinessLogic.Validators;
using LibraryManagement.DataAccess.Models;
using LibraryManagement.DataAccess.Repositories;

namespace LibraryManagement.BusinessLogic.Services;

public class BookService(IBookRepository bookRepository, IAuthorRepository authorRepository) : IBookService
{
    public async Task<IEnumerable<BookDto>> GetAllAsync(int? publishedAfterYear = null)
    {
        if (publishedAfterYear.HasValue && (publishedAfterYear < 0 || publishedAfterYear > DateTime.Now.Year))
            throw new ArgumentException("Некорректный год");

        var books = await bookRepository.GetAllAsync(publishedAfterYear);
        return books.Select(MapToDto);
    }

    public async Task<BookDto?> GetByIdAsync(int id)
    {
        var book = await bookRepository.GetByIdAsync(id);
        return book is null ? null : MapToDto(book);
    }

    public async Task<BookDto> CreateAsync(CreateBookDto dto)
    {
        var yearValidation = BookValidator.ValidatePublishedYear(dto.PublishedYear);
        if (!yearValidation.IsValid)
            throw new ArgumentException(yearValidation.ErrorMessage);

        var author = await authorRepository.GetByIdAsync(dto.AuthorId);
        if (author is null)
            throw new ArgumentException($"Автор с ID {dto.AuthorId} не найден");

        var book = new Book
        {
            Title = dto.Title,
            PublishedYear = dto.PublishedYear,
            AuthorId = dto.AuthorId
        };

        var createdBook = await bookRepository.CreateAsync(book);
        
        createdBook.Author = author;
        return MapToDto(createdBook);
    }

    public async Task<bool> UpdateAsync(int id, UpdateBookDto dto)
    {
        var yearValidation = BookValidator.ValidatePublishedYear(dto.PublishedYear);
        if (!yearValidation.IsValid)
            throw new ArgumentException(yearValidation.ErrorMessage);

        var author = await authorRepository.GetByIdAsync(dto.AuthorId);
        if (author is null)
            throw new ArgumentException($"Автор с ID {dto.AuthorId} не найден");

        var book = new Book
        {
            Title = dto.Title,
            PublishedYear = dto.PublishedYear,
            AuthorId = dto.AuthorId
        };

        return await bookRepository.UpdateAsync(id, book);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await bookRepository.DeleteAsync(id);
    }

    private static BookDto MapToDto(Book book)
    {
        return new BookDto
        {
            Id = book.Id,
            Title = book.Title,
            PublishedYear = book.PublishedYear,
            AuthorId = book.AuthorId,
            AuthorName = book.Author?.Name ?? string.Empty
        };
    }
}
