using Microsoft.EntityFrameworkCore;
using BOOK_API.Data;
using BOOK_API.Models;
using BOOK_API.DTOs;

namespace BOOK_API.Services
{
    public class BookService
    {
        private string connectionStr = "Server=tcp:epita.database.windows.net,1433;Initial Catalog=EPITA-C#;Persist Security Info=False;User ID=ousama;Password=terry.Zoro69;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        private AppDbContext GetDbContext()
        {
            var builder = new DbContextOptionsBuilder<AppDbContext>();
            builder.UseSqlServer(connectionStr);
            return new AppDbContext(builder.Options);
        }

        public async Task<List<BookDTO>> GetAllBooks()
        {
            var context = GetDbContext();
            var books = await context.Books.Include(b => b.Author).ToListAsync();

            var result = new List<BookDTO>();
            foreach (var book in books)
            {
                var dto = new BookDTO
                {
                    BookId = book.BookId,
                    Title = book.Title,
                    Genre = book.Genre,
                    ISBN = book.ISBN,
                    PublishedDate = book.PublishedDate,
                    AuthorId = book.AuthorId,
                    AuthorName = book.Author.FirstName + " " + book.Author.LastName
                };
                result.Add(dto);
            }

            context.Dispose();
            return result;
        }

        public async Task<BookDTO> GetBookById(int id)
        {
            var context = GetDbContext();
            var book = await context.Books
                .Include(b => b.Author)
                .FirstOrDefaultAsync(b => b.BookId == id);

            if (book == null)
            {
                context.Dispose();
                return null;
            }

            var dto = new BookDTO
            {
                BookId = book.BookId,
                Title = book.Title,
                Genre = book.Genre,
                ISBN = book.ISBN,
                PublishedDate = book.PublishedDate,
                AuthorId = book.AuthorId,
                AuthorName = book.Author.FirstName + " " + book.Author.LastName
            };

            context.Dispose();
            return dto;
        }

        public async Task<BookDTO> CreateBook(BookInputDTO input)
        {
            var context = GetDbContext();

            var book = new Book();
            book.Title = input.Title;
            book.Genre = input.Genre;
            book.ISBN = input.ISBN;
            book.PublishedDate = input.PublishedDate;
            book.AuthorId = input.AuthorId;

            context.Books.Add(book);
            await context.SaveChangesAsync();

            var created = await context.Books
                .Include(b => b.Author)
                .FirstOrDefaultAsync(b => b.BookId == book.BookId);

            var dto = new BookDTO
            {
                BookId = created.BookId,
                Title = created.Title,
                Genre = created.Genre,
                ISBN = created.ISBN,
                PublishedDate = created.PublishedDate,
                AuthorId = created.AuthorId,
                AuthorName = created.Author.FirstName + " " + created.Author.LastName
            };

            context.Dispose();
            return dto;
        }

        public async Task<bool> UpdateBook(int id, BookInputDTO input)
        {
            var context = GetDbContext();
            var book = await context.Books.FindAsync(id);

            if (book == null)
            {
                context.Dispose();
                return false;
            }

            book.Title = input.Title;
            book.Genre = input.Genre;
            book.ISBN = input.ISBN;
            book.PublishedDate = input.PublishedDate;
            book.AuthorId = input.AuthorId;

            await context.SaveChangesAsync();
            context.Dispose();
            return true;
        }

        public async Task<bool> DeleteBook(int id)
        {
            var context = GetDbContext();
            var book = await context.Books.FindAsync(id);

            if (book == null)
            {
                context.Dispose();
                return false;
            }

            context.Books.Remove(book);
            await context.SaveChangesAsync();
            context.Dispose();
            return true;
        }

        public async Task<List<BookDTO>> GetBooksByAuthor(int authorId)
        {
            var context = GetDbContext();
            var books = await context.Books
                .Include(b => b.Author)
                .Where(b => b.AuthorId == authorId)
                .ToListAsync();

            var result = new List<BookDTO>();
            for (int i = 0; i < books.Count; i++)
            {
                var book = books[i];
                var dto = new BookDTO
                {
                    BookId = book.BookId,
                    Title = book.Title,
                    Genre = book.Genre,
                    ISBN = book.ISBN,
                    PublishedDate = book.PublishedDate,
                    AuthorId = book.AuthorId,
                    AuthorName = book.Author.FirstName + " " + book.Author.LastName
                };
                result.Add(dto);
            }

            context.Dispose();
            return result;
        }
    }
}