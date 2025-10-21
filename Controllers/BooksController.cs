using Microsoft.AspNetCore.Mvc;
using BOOK_API.DTOs;
using BOOK_API.Services;

namespace BOOK_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly BookService _bookService; // Store injected service

        // Constructor Injection
        public BooksController(BookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllBooks()
        {
            var books = await _bookService.GetAllBooks();
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetBookById(int id)
        {
            var book = await _bookService.GetBookById(id);

            if (book == null)
                return NotFound("Book not found");

            return Ok(book);
        }

        [HttpGet("author/{authorId}")]
        public async Task<ActionResult> GetBooksByAuthor(int authorId)
        {
            var books = await _bookService.GetBooksByAuthor(authorId);
            return Ok(books);
        }

        [HttpPost]
        public async Task<ActionResult> CreateBook([FromBody] BookInputDTO input)
        {
            if (string.IsNullOrEmpty(input.Title))
                return BadRequest("Title is required");

            var created = await _bookService.CreateBook(input);

            return CreatedAtAction(nameof(GetBookById), new { id = created.BookId }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateBook(int id, [FromBody] BookInputDTO input)
        {
            if (string.IsNullOrEmpty(input.Title))
                return BadRequest("Title is required");

            var updated = await _bookService.UpdateBook(id, input);

            if (!updated)
                return NotFound("Book not found");

            return Ok("Book updated");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBook(int id)
        {
            var deleted = await _bookService.DeleteBook(id);

            if (!deleted)
                return NotFound("Book not found");

            return Ok("Book deleted");
        }
    }
}