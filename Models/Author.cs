namespace BOOK_API.Models
{
    public class Author
    {
        public int AuthorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTimeOffset? BirthDate { get; set; }  // CHANGED
        public string Nationality { get; set; }
        public string Biography { get; set; }
        public ICollection<Book> Books { get; set; }
    }
}