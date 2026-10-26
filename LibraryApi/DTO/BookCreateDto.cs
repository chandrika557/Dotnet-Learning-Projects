using System.ComponentModel.DataAnnotations;

namespace LibraryApi.DTOs;

public class BookCreateDto
{
    [Required, StringLength(200, MinimumLength = 1)]
    public string Title { get; set; } = string.Empty;

    [StringLength(20)]
    public string? Isbn { get; set; }

    [Range(1000, 2100)]
    public int PublishedYear { get; set; }

    [Range(typeof(decimal), "0.01", "100000")]
    public decimal Price { get; set; }

    [Range(1, int.MaxValue)]
    public int AuthorId { get; set; }
}