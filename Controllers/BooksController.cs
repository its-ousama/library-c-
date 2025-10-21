using Microsoft.AspNetCore.Mvc;
using BOOK_API.DTOs;
using BOOK_API.Services;

namespace BOOK_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> GetAllBooks()
        {
            var service = new BookService();
            var books = await service.GetAllBooks();
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetBookById(int id)
        {
            var service = new BookService();
            var book = await service.GetBookById(id);

            if (book == null)
                return NotFound("Book not found");

            return Ok(book);
        }

        [HttpGet("author/{authorId}")]
        public async Task<ActionResult> GetBooksByAuthor(int authorId)
        {
            var service = new BookService();
            var books = await service.GetBooksByAuthor(authorId);
            return Ok(books);
        }

        [HttpPost]
        public async Task<ActionResult> CreateBook([FromBody] BookInputDTO input)
        {
            if (string.IsNullOrEmpty(input.Title))
                return BadRequest("Title is required");

            var service = new BookService();
            var created = await service.CreateBook(input);

            return CreatedAtAction(nameof(GetBookById), new { id = created.BookId }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateBook(int id, [FromBody] BookInputDTO input)
        {
            if (string.IsNullOrEmpty(input.Title))
                return BadRequest("Title is required");

            var service = new BookService();
            var updated = await service.UpdateBook(id, input);

            if (!updated)
                return NotFound("Book not found");

            return Ok("Book updated");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBook(int id)
        {
            var service = new BookService();
            var deleted = await service.DeleteBook(id);

            if (!deleted)
                return NotFound("Book not found");

            return Ok("Book deleted");
        }
    }
}