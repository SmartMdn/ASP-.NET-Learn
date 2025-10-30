using System.ComponentModel.DataAnnotations;

namespace LibraryManagementAPI.DTOs;

public class CreateBookDto
{
    [Required(ErrorMessage = "Название книги обязательно")]
    [StringLength(500, MinimumLength = 1, ErrorMessage = "Название книги должно быть от 1 до 500 символов")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Год публикации обязателен")]
    [Range(1, int.MaxValue, ErrorMessage = "Год публикации должен быть положительным числом")]
    public int PublishedYear { get; set; }

    [Required(ErrorMessage = "ID автора обязателен")]
    public int AuthorId { get; set; }
}
