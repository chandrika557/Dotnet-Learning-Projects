using System.ComponentModel.DataAnnotations; //model validation attributes are used to enforce rules on the properties of the DTO classes, such as required fields, string length limits, and range constraints.

namespace LibraryApi.DTOs;

public class AuthorCreateDto
{
    [Required, StringLength(100, MinimumLength = 1)]
    public string Name { get; set; } = string.Empty;
}

public class AuthorResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}