using Microsoft.AspNetCore.Mvc;
using BOOK_API.DTOs;
using BOOK_API.Services;

namespace BOOK_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorsController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> GetAllAuthors()
        {
            var service = new AuthorService();
            var authors = await service.GetAllAuthors();
            return Ok(authors);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetAuthorById(int id)
        {
            var service = new AuthorService();
            var author = await service.GetAuthorById(id);

            if (author == null)
                return NotFound("Author not found");

            return Ok(author);
        }

        [HttpPost]
        public async Task<ActionResult> CreateAuthor([FromBody] AuthorInputDTO input)
        {
            if (string.IsNullOrEmpty(input.FirstName) || string.IsNullOrEmpty(input.LastName))
                return BadRequest("FirstName and LastName are required");

            var service = new AuthorService();
            var created = await service.CreateAuthor(input);

            return CreatedAtAction(nameof(GetAuthorById), new { id = created.AuthorId }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAuthor(int id, [FromBody] AuthorInputDTO input)
        {
            if (string.IsNullOrEmpty(input.FirstName) || string.IsNullOrEmpty(input.LastName))
                return BadRequest("FirstName and LastName are required");

            var service = new AuthorService();
            var updated = await service.UpdateAuthor(id, input);

            if (!updated)
                return NotFound("Author not found");

            return Ok("Author updated");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAuthor(int id)
        {
            var service = new AuthorService();
            var deleted = await service.DeleteAuthor(id);

            if (!deleted)
                return NotFound("Author not found");

            return Ok("Author deleted");
        }
    }
}