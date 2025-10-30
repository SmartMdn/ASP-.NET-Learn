namespace LibraryManagementAPI.Validators;

public static class AuthorValidator
{
    public static (bool IsValid, string? ErrorMessage) ValidateDateOfBirth(DateTime dateOfBirth)
    {
        if (dateOfBirth > DateTime.Now)
        {
            return (false, "Дата рождения не может быть в будущем");
        }

        if (dateOfBirth < new DateTime(1000, 1, 1))
        {
            return (false, "Дата рождения слишком давняя");
        }

        return (true, null);
    }
}
