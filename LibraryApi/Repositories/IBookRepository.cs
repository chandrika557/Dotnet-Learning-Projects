using LibraryApi.Models;

namespace LibraryApi.Repositories; // Defines the contract for a repository that manages Book entities in the database

public interface IBookRepository // Defines the contract for a repository that manages Book entities in the database
{
    Task<List<Book>> GetAllAsync(); // Retrieves all books from the database, including their associated authors, without tracking changes to the entities
    Task<Book?> GetByIdAsync(int id);//
    Task<Book> AddAsync(Book book); // Adds a new book to the database
    Task<bool> UpdateAsync(Book book); // Updates an existing book in the database
    Task<bool> DeleteAsync(int id);//
    Task<bool> AuthorExistsAsync(int authorId); //
}