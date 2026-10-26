using LibraryApi.Data;
using LibraryApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Repositories;

public class BookRepository : IBookRepository // Implementation of the IBookRepository interface for managing Book entities in the database
{
    private readonly AppDbContext _context; // Dependency injection of the AppDbContext to interact with the database

    public BookRepository(AppDbContext context) => _context = context; // Constructor that accepts an AppDbContext instance and assigns it to the private field _context

    public async Task<List<Book>> GetAllAsync() =>
        await _context.Books.AsNoTracking().Include(b => b.Author).ToListAsync(); //

    public async Task<Book?> GetByIdAsync(int id) => //aasync method usually returns a Task<T> or Task, where T is the type of the result. In this case, it returns a Task<Book?>, which means it will eventually return a Book object or null. The method uses the _context.Books DbSet to query the database for a book with the specified id. It uses AsNoTracking() to improve performance by not tracking changes to the entities, and Include(b => b.Author) to eagerly load the related Author entity for each Book. Finally, it uses FirstOrDefaultAsync(b => b.Id == id) to return the first book that matches the specified id or null if no such book exists.
        await _context.Books.AsNoTracking().Include(b => b.Author) //asNoTracking() is used to improve performance by not tracking changes to the entities, and Include(b => b.Author) is used to eagerly load the related Author entity for each Book.
            .FirstOrDefaultAsync(b => b.Id == id); 

    public async Task<Book> AddAsync(Book book) //as
    {
        _context.Books.Add(book);
        await _context.SaveChangesAsync();
        return book;
    }

    public async Task<bool> UpdateAsync(Book book)
    {
        var existing = await _context.Books.FindAsync(book.Id);
        if (existing is null) return false;

        existing.Title = book.Title;
        existing.Isbn = book.Isbn;
        existing.PublishedYear = book.PublishedYear;
        existing.Price = book.Price;
        existing.AuthorId = book.AuthorId;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book is null) return false;

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AuthorExistsAsync(int authorId) =>
        await _context.Authors.AnyAsync(a => a.Id == authorId);
}