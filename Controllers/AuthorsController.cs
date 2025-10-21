using Microsoft.AspNetCore.Mvc;
using BOOK_API.DTOs;
using BOOK_API.Services;

namespace BOOK_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorsController : ControllerBase
    {
        private readonly AuthorService _authorService; // Store injected service

        // Constructor Injection
        public AuthorsController(AuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllAuthors()
        {
            var authors = await _authorService.GetAllAuthors();
            return Ok(authors);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetAuthorById(int id)
        {
            var author = await _authorService.GetAuthorById(id);

            if (author == null)
                return NotFound("Author not found");

            return Ok(author);
        }

        [HttpPost]
        public async Task<ActionResult> CreateAuthor([FromBody] AuthorInputDTO input)
        {
            if (string.IsNullOrEmpty(input.FirstName) || string.IsNullOrEmpty(input.LastName))
                return BadRequest("FirstName and LastName are required");

            var created = await _authorService.CreateAuthor(input);

            return CreatedAtAction(nameof(GetAuthorById), new { id = created.AuthorId }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAuthor(int id, [FromBody] AuthorInputDTO input)
        {
            if (string.IsNullOrEmpty(input.FirstName) || string.IsNullOrEmpty(input.LastName))
                return BadRequest("FirstName and LastName are required");

            var updated = await _authorService.UpdateAuthor(id, input);

            if (!updated)
                return NotFound("Author not found");

            return Ok("Author updated");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAuthor(int id)
        {
            var deleted = await _authorService.DeleteAuthor(id);

            if (!deleted)
                return NotFound("Author not found");

            return Ok("Author deleted");
        }
    }
}