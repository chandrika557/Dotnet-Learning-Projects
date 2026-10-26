namespace LibraryApi.Models;

public class Author
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    // One author has many books
    public List<Book> Books { get; set; } = new();
}