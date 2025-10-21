namespace BOOK_API.DTOs
{
    
    public class BookDTO
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public string ISBN { get; set; }
        public DateTime? PublishedDate { get; set; }
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
    }

    
    public class BookInputDTO
    {
        public string Title { get; set; }
        public string Genre { get; set; }
        public string ISBN { get; set; }
        public DateTime? PublishedDate { get; set; }
        public int AuthorId { get; set; }
    }
}