using LibraryApi.DTOs;
using LibraryApi.Mappings;
using LibraryApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookRepository _repo;

    public BooksController(IBookRepository repo) => _repo = repo;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookResponseDto>>> GetAll()
    {
        var books = await _repo.GetAllAsync();
        return Ok(books.Select(b => b.ToResponse()));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<BookResponseDto>> GetById(int id)
    {
        var book = await _repo.GetByIdAsync(id);
        return book is null ? NotFound() : Ok(book.ToResponse());
    }

    [HttpPost]
    public async Task<ActionResult<BookResponseDto>> Create(BookCreateDto dto)
    {
        if (!await _repo.AuthorExistsAsync(dto.AuthorId))
            return BadRequest($"Author {dto.AuthorId} does not exist.");

        var created = await _repo.AddAsync(dto.ToEntity());
        var saved = await _repo.GetByIdAsync(created.Id); // reload so Author is included
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, saved!.ToResponse());
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, BookUpdateDto dto)
    {
        if (!await _repo.AuthorExistsAsync(dto.AuthorId))
            return BadRequest($"Author {dto.AuthorId} does not exist.");

        var book = dto.ToEntity();
        book.Id = id;
        return await _repo.UpdateAsync(book) ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id) =>
        await _repo.DeleteAsync(id) ? NoContent() : NotFound();

    [HttpGet("test-error")]
    public IActionResult TestError() => throw new Exception("Boom! Something broke.");

    [HttpGet("test-notfound")]
    public IActionResult TestNotFound() => throw new LibraryApi.Exceptions.NotFoundException("Book not found for testing.");
}