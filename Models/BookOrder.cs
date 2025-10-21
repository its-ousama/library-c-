namespace BOOK_API.Models
{
    public class BookOrder
    {
        public int BookOrderId { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime OrderDate { get; set; }
        public Book Book { get; set; }
    }
}