using LibraryApi.DTOs;
using LibraryApi.Models;

namespace LibraryApi.Mappings;

public static class BookMappings
{
    public static BookResponseDto ToResponse(this Book b) => new() //toresponse method is an extension method that converts a Book entity to a BookResponseDto. It maps the properties of the Book entity to the corresponding properties of the BookResponseDto, including the AuthorName property, which is obtained from the related Author entity.
    {
        Id = b.Id,
        Title = b.Title,
        Isbn = b.Isbn,
        PublishedYear = b.PublishedYear,
        Price = b.Price,
        AuthorId = b.AuthorId,
        AuthorName = b.Author?.Name ?? string.Empty
    };

    public static Book ToEntity(this BookCreateDto d) => new()
    {
        Title = d.Title,
        Isbn = d.Isbn,
        PublishedYear = d.PublishedYear,
        Price = d.Price,
        AuthorId = d.AuthorId
    };
}