using LibraryManagement.BusinessLogic.Models;

namespace LibraryManagement.BusinessLogic.Validators;

public static class AuthorValidator
{
    public static ValidationResult ValidateDateOfBirth(DateTime dateOfBirth)
    {
        if (dateOfBirth > DateTime.Now)
            return ValidationResult.Failure("Дата рождения не может быть в будущем");

        if (dateOfBirth < new DateTime(1000, 1, 1))
            return ValidationResult.Failure("Дата рождения слишком давняя");

        return ValidationResult.Success();
    }
}
