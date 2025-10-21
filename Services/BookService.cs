using Microsoft.EntityFrameworkCore;
using BOOK_API.Data;
using BOOK_API.Models;
using BOOK_API.DTOs;
using System.Linq;

namespace BOOK_API.Services
{
    public class BookService
    {
        private readonly AppDbContext _context;

        // Constructor Injection
        public BookService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<BookDTO>> GetAllBooks()
        {
            var books = await _context.Books
                .Include(b => b.Author)
                .ToListAsync();

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
                    // Use null-conditional access (?.)
                    AuthorName = book.Author?.FirstName + " " + book.Author?.LastName
                };
                result.Add(dto);
            }

            return result;
        }

        // Changed return type to BookDTO? to fix warning CS8603
        public async Task<BookDTO?> GetBookById(int id)
        {
            var book = await _context.Books
                .Include(b => b.Author)
                .FirstOrDefaultAsync(b => b.BookId == id);

            if (book == null)
            {
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
                AuthorName = book.Author?.FirstName + " " + book.Author?.LastName
            };

            return dto;
        }

        public async Task<BookDTO> CreateBook(BookInputDTO input)
        {
            var book = new Book();
            book.Title = input.Title;
            book.Genre = input.Genre;
            book.ISBN = input.ISBN;

            // FIX for CS0029: Safely cast to DateTimeOffset? and extract DateTime
            book.PublishedDate = ((DateTimeOffset?)input.PublishedDate)?.DateTime;

            book.AuthorId = input.AuthorId;
            book.PublisherId = 1; // Assuming default PublisherId = 1 as per AppDbContext seed data

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            var created = await _context.Books
                .Include(b => b.Author)
                .FirstOrDefaultAsync(b => b.BookId == book.BookId);

            if (created == null) return new BookDTO();

            var dto = new BookDTO
            {
                BookId = created.BookId,
                Title = created.Title,
                Genre = created.Genre,
                ISBN = created.ISBN,
                PublishedDate = created.PublishedDate,
                AuthorId = created.AuthorId,
                AuthorName = created.Author?.FirstName + " " + created.Author?.LastName
            };

            return dto;
        }

        public async Task<bool> UpdateBook(int id, BookInputDTO input)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return false;
            }

            book.Title = input.Title;
            book.Genre = input.Genre;
            book.ISBN = input.ISBN;

            // FIX for CS0029: Safely cast to DateTimeOffset? and extract DateTime
            book.PublishedDate = ((DateTimeOffset?)input.PublishedDate)?.DateTime;

            book.AuthorId = input.AuthorId;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return false;
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<BookDTO>> GetBooksByAuthor(int authorId)
        {
            var books = await _context.Books
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
                    AuthorName = book.Author?.FirstName + " " + book.Author?.LastName
                };
                result.Add(dto);
            }

            return result;
        }
    }
}