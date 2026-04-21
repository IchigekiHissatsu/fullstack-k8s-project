using Microsoft.AspNetCore.Mvc;
using MyBackendAPI.Models;
using MyBackendAPI.Services;

namespace MyBackendAPI.Controllers;

[ApiController] // <-- EZ A NAGYON FONTOS SOR!
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly BookService _bookService;

    public BooksController(BookService bookService) =>
        _bookService = bookService;

    [HttpGet]
    public async Task<List<Book>> Get() => await _bookService.GetAsync();

    [HttpPost]
    public async Task<IActionResult> Post(Book newBook)
    {
        await _bookService.CreateAsync(newBook);
        return CreatedAtAction(nameof(Get), new { id = newBook.Id }, newBook);
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _bookService.RemoveAsync(id);
        return NoContent();
    }
}