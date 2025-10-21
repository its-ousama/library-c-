namespace BOOK_API.DTOs
{
  
    public class AuthorDTO
    {
        public int AuthorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Nationality { get; set; }
        public string Biography { get; set; }
        public List<BookDTO> Books { get; set; }
    }

    
    public class AuthorInputDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Nationality { get; set; }
        public string Biography { get; set; }
    }
}
