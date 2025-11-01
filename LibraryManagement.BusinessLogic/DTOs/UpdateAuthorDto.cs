using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.BusinessLogic.DTOs;

public class UpdateAuthorDto
{
    [Required(ErrorMessage = "Имя автора обязательно")]
    [StringLength(200, MinimumLength = 1, ErrorMessage = "Имя автора должно быть от 1 до 200 символов")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Дата рождения обязательна")]
    public DateTime DateOfBirth { get; set; }
}
