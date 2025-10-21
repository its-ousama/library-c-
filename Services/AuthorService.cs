using Microsoft.EntityFrameworkCore;
using BOOK_API.Data;
using BOOK_API.Models;
using BOOK_API.DTOs;

namespace BOOK_API.Services
{
    public class AuthorService
    {
        private string connectionStr = "Server=tcp:epita.database.windows.net,1433;Initial Catalog=EPITA-C#;Persist Security Info=False;User ID=ousama;Password=terry.Zoro69;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        private AppDbContext GetDbContext()
        {
            var builder = new DbContextOptionsBuilder<AppDbContext>();
            builder.UseSqlServer(connectionStr);
            return new AppDbContext(builder.Options);
        }

        public async Task<List<AuthorDTO>> GetAllAuthors()
        {
            var context = GetDbContext();
            var authors = await context.Authors.Include(a => a.Books).ToListAsync();

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

            context.Dispose();
            return result;
        }

        public async Task<AuthorDTO> GetAuthorById(int id)
        {
            var context = GetDbContext();
            var author = await context.Authors
                .Include(a => a.Books)
                .FirstOrDefaultAsync(a => a.AuthorId == id);

            if (author == null)
            {
                context.Dispose();
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

            context.Dispose();
            return dto;
        }

        public async Task<AuthorDTO> CreateAuthor(AuthorInputDTO input)
        {
            var context = GetDbContext();

            var author = new Author();
            author.FirstName = input.FirstName;
            author.LastName = input.LastName;
            author.BirthDate = input.BirthDate;
            author.Nationality = input.Nationality;
            author.Biography = input.Biography;

            context.Authors.Add(author);
            await context.SaveChangesAsync();

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

            context.Dispose();
            return dto;
        }

        public async Task<bool> UpdateAuthor(int id, AuthorInputDTO input)
        {
            var context = GetDbContext();
            var author = await context.Authors.FindAsync(id);

            if (author == null)
            {
                context.Dispose();
                return false;
            }

            author.FirstName = input.FirstName;
            author.LastName = input.LastName;
            author.BirthDate = input.BirthDate;
            author.Nationality = input.Nationality;
            author.Biography = input.Biography;

            await context.SaveChangesAsync();
            context.Dispose();
            return true;
        }

        public async Task<bool> DeleteAuthor(int id)
        {
            var context = GetDbContext();
            var author = await context.Authors.FindAsync(id);

            if (author == null)
            {
                context.Dispose();
                return false;
            }

            context.Authors.Remove(author);
            await context.SaveChangesAsync();
            context.Dispose();
            return true;
        }
    }
}