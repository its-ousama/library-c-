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
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<BookOrder> BookOrders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Existing Author - Book relationship
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            // NEW Publisher - Book relationship
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Publisher)
                .WithMany(p => p.Books)
                .HasForeignKey(b => b.PublisherId)
                .OnDelete(DeleteBehavior.Restrict);

            // NEW Book - BookOrder relationship
            modelBuilder.Entity<BookOrder>()
                .HasOne(bo => bo.Book)
                .WithMany(b => b.Orders)
                .HasForeignKey(bo => bo.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            // Add Seed Data for Publisher
            modelBuilder.Entity<Publisher>().HasData(
                new Publisher { PublisherId = 1, Name = "Secker & Warburg", Country = "UK" },
                new Publisher { PublisherId = 2, Name = "Bloomsbury Publishing", Country = "UK" }
            );

            // Author Seed Data (DATES FIXED)
            modelBuilder.Entity<Author>().HasData(
                new Author
                {
                    AuthorId = 1,
                    FirstName = "George",
                    LastName = "Orwell",
                    // FIX: Added DateTimeKind.Utc
                    BirthDate = new DateTime(1903, 6, 25, 0, 0, 0, DateTimeKind.Utc),
                    Nationality = "British",
                    Biography = "English novelist and essayist"
                },
                new Author
                {
                    AuthorId = 2,
                    FirstName = "J.K.",
                    LastName = "Rowling",
                    // FIX: Added DateTimeKind.Utc
                    BirthDate = new DateTime(1965, 7, 31, 0, 0, 0, DateTimeKind.Utc),
                    Nationality = "British",
                    Biography = "Author of Harry Potter series"
                }
            );

            // Book Seed Data (DATES FIXED)
            modelBuilder.Entity<Book>().HasData(
                new Book
                {
                    BookId = 1,
                    Title = "1984",
                    Genre = "Dystopian",
                    ISBN = "978-0451524935",
                    // FIX: Added DateTimeKind.Utc
                    PublishedDate = new DateTime(1949, 6, 8, 0, 0, 0, DateTimeKind.Utc),
                    AuthorId = 1,
                    PublisherId = 1
                },
                new Book
                {
                    BookId = 2,
                    Title = "Animal Farm",
                    Genre = "Political Satire",
                    ISBN = "978-0451526342",
                    // FIX: Added DateTimeKind.Utc
                    PublishedDate = new DateTime(1945, 8, 17, 0, 0, 0, DateTimeKind.Utc),
                    AuthorId = 1,
                    PublisherId = 1
                }
            );

            // BookOrder Seed Data (DATES FIXED)
            modelBuilder.Entity<BookOrder>().HasData(
                new BookOrder
                {
                    BookOrderId = 1,
                    BookId = 1,
                    Quantity = 10,
                    Price = 12.99m,
                    // FIX: Added DateTimeKind.Utc
                    OrderDate = new DateTime(2025, 10, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new BookOrder
                {
                    BookOrderId = 2,
                    BookId = 2,
                    Quantity = 5,
                    Price = 9.99m,
                    // FIX: Added DateTimeKind.Utc
                    OrderDate = new DateTime(2025, 10, 5, 0, 0, 0, DateTimeKind.Utc)
                }
            );
        }
    }
}