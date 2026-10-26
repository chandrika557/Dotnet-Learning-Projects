using LibraryApi.Data;
using LibraryApi.DTOs;
using LibraryApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorsController : ControllerBase
{
    private readonly AppDbContext _context;

    public AuthorsController(AppDbContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AuthorResponseDto>>> GetAll() =>
        Ok(await _context.Authors.AsNoTracking()
            .Select(a => new AuthorResponseDto { Id = a.Id, Name = a.Name })
            .ToListAsync());

    [HttpPost]
    public async Task<ActionResult<AuthorResponseDto>> Create(AuthorCreateDto dto)
    {
        var author = new Author { Name = dto.Name };
        _context.Authors.Add(author);
        await _context.SaveChangesAsync();
        return Ok(new AuthorResponseDto { Id = author.Id, Name = author.Name });
    }
}