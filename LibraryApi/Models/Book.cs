namespace LibraryApi.Models;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Isbn { get; set; }
    public int PublishedYear { get; set; }
    public decimal Price { get; set; }

    // Foreign key + navigation property
    public int AuthorId { get; set; }
    public Author? Author { get; set; } 
}