namespace LibraryManagement.BusinessLogic.Models;

public class ValidationResult
{
    public bool IsValid { get; }
    public string? ErrorMessage { get; }

    private ValidationResult(bool isValid, string? errorMessage)
    {
        IsValid = isValid;
        ErrorMessage = errorMessage;
    }

    public static ValidationResult Success() => new(true, null);
    
    public static ValidationResult Failure(string errorMessage) => new(false, errorMessage);
    
    public void Deconstruct(out bool isValid, out string? errorMessage)
    {
        isValid = IsValid;
        errorMessage = ErrorMessage;
    }
}
