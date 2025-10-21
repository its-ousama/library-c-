using BOOK_API.Models;

namespace BOOK_API.Models
{
    public class Book
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public string ISBN { get; set; }
        public DateTime? PublishedDate { get; set; }

        // Author relationship (Existing)
        public int AuthorId { get; set; }
        public Author Author { get; set; }

        // NEW Publisher relationship
        public int PublisherId { get; set; }
        public Publisher Publisher { get; set; }

        // NEW BookOrders relationship (one-to-many)
        public ICollection<BookOrder> Orders { get; set; }
    }
}