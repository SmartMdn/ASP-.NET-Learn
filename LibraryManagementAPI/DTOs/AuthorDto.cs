namespace LibraryManagementAPI.DTOs;

public class AuthorDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
}
