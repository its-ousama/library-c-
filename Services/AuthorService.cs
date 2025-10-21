using Microsoft.EntityFrameworkCore;
using BOOK_API.Data;
using BOOK_API.Models;
using BOOK_API.DTOs;
using System.Linq;

namespace BOOK_API.Services
{
    public class AuthorService
    {
        private readonly AppDbContext _context;

        // Constructor Injection
        public AuthorService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<AuthorDTO>> GetAllAuthors()
        {
            var authors = await _context.Authors.Include(a => a.Books).ToListAsync();

            var result = new List<AuthorDTO>();
            foreach (var author in authors)
            {
                var dto = new AuthorDTO
                {
                    AuthorId = author.AuthorId,
                    FirstName = author.FirstName,
                    LastName = author.LastName,
                    BirthDate = author.BirthDate,
                    Nationality = author.Nationality,
                    Biography = author.Biography,
                    Books = new List<BookDTO>()
                };

                foreach (var book in author.Books)
                {
                    var bookDto = new BookDTO
                    {
                        BookId = book.BookId,
                        Title = book.Title,
                        Genre = book.Genre,
                        ISBN = book.ISBN,
                        PublishedDate = book.PublishedDate,
                        AuthorId = book.AuthorId,
                        AuthorName = author.FirstName + " " + author.LastName
                    };
                    dto.Books.Add(bookDto);
                }

                result.Add(dto);
            }

            return result;
        }

        // Changed return type to AuthorDTO? to fix warning CS8603
        public async Task<AuthorDTO?> GetAuthorById(int id)
        {
            var author = await _context.Authors
                .Include(a => a.Books)
                .FirstOrDefaultAsync(a => a.AuthorId == id);

            if (author == null)
            {
                return null;
            }

            var dto = new AuthorDTO
            {
                AuthorId = author.AuthorId,
                FirstName = author.FirstName,
                LastName = author.LastName,
                BirthDate = author.BirthDate,
                Nationality = author.Nationality,
                Biography = author.Biography,
                Books = new List<BookDTO>()
            };

            foreach (var book in author.Books)
            {
                var bookDto = new BookDTO
                {
                    BookId = book.BookId,
                    Title = book.Title,
                    Genre = book.Genre,
                    ISBN = book.ISBN,
                    PublishedDate = book.PublishedDate,
                    AuthorId = book.AuthorId,
                    AuthorName = author.FirstName + " " + author.LastName
                };
                dto.Books.Add(bookDto);
            }

            return dto;
        }

        public async Task<AuthorDTO> CreateAuthor(AuthorInputDTO input)
        {
            var author = new Author();
            author.FirstName = input.FirstName;
            author.LastName = input.LastName;
            author.BirthDate = input.BirthDate;
            author.Nationality = input.Nationality;
            author.Biography = input.Biography;

            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            var dto = new AuthorDTO
            {
                AuthorId = author.AuthorId,
                FirstName = author.FirstName,
                LastName = author.LastName,
                BirthDate = input.BirthDate,
                Nationality = input.Nationality,
                Biography = input.Biography,
                Books = new List<BookDTO>()
            };

            return dto;
        }

        public async Task<bool> UpdateAuthor(int id, AuthorInputDTO input)
        {
            var author = await _context.Authors.FindAsync(id);

            if (author == null)
            {
                return false;
            }

            author.FirstName = input.FirstName;
            author.LastName = input.LastName;
            author.BirthDate = input.BirthDate;
            author.Nationality = input.Nationality;
            author.Biography = input.Biography;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAuthor(int id)
        {
            var author = await _context.Authors.FindAsync(id);

            if (author == null)
            {
                return false;
            }

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}