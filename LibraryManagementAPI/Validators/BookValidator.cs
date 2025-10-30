namespace LibraryManagementAPI.Validators;

public static class BookValidator
{
    public static (bool IsValid, string? ErrorMessage) ValidatePublishedYear(int publishedYear)
    {
        var currentYear = DateTime.Now.Year;

        if (publishedYear < 0)
        {
            return (false, "Год публикации не может быть отрицательным");
        }

        if (publishedYear > currentYear)
        {
            return (false, "Год публикации не может быть в будущем");
        }

        return (true, null);
    }
}
