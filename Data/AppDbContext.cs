using Microsoft.EntityFrameworkCore;
using BOOK_API.Models;

namespace BOOK_API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // setup relationship between books and authors
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            // add some test data
            modelBuilder.Entity<Author>().HasData(
                new Author
                {
                    AuthorId = 1,
                    FirstName = "George",
                    LastName = "Orwell",
                    BirthDate = new DateTime(1903, 6, 25),
                    Nationality = "British",
                    Biography = "English novelist and essayist"
                },
                new Author
                {
                    AuthorId = 2,
                    FirstName = "J.K.",
                    LastName = "Rowling",
                    BirthDate = new DateTime(1965, 7, 31),
                    Nationality = "British",
                    Biography = "Author of Harry Potter series"
                }
            );

            modelBuilder.Entity<Book>().HasData(
                new Book
                {
                    BookId = 1,
                    Title = "1984",
                    Genre = "Dystopian",
                    ISBN = "978-0451524935",
                    PublishedDate = new DateTime(1949, 6, 8),
                    AuthorId = 1
                },
                new Book
                {
                    BookId = 2,
                    Title = "Animal Farm",
                    Genre = "Political Satire",
                    ISBN = "978-0451526342",
                    PublishedDate = new DateTime(1945, 8, 17),
                    AuthorId = 1
                }
            );
        }
    }
}