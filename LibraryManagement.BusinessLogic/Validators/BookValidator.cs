using LibraryManagement.BusinessLogic.Models;

namespace LibraryManagement.BusinessLogic.Validators;

public static class BookValidator
{
    public static ValidationResult ValidatePublishedYear(int publishedYear)
    {
        var currentYear = DateTime.Now.Year;

        if (publishedYear < 0)
            return ValidationResult.Failure("Год публикации не может быть отрицательным");

        if (publishedYear > currentYear)
            return ValidationResult.Failure("Год публикации не может быть в будущем");

        return ValidationResult.Success();
    }
}
