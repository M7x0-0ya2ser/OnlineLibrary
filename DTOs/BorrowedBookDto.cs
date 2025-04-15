namespace OnlineLibrary.DTOs
{
    using System.ComponentModel.DataAnnotations;


    public class BorrowedBookDto
    {
        [DataType(DataType.Date)]
        public DateOnly? DateOfReturn { get; set; }
        public int OrderNumber { get; set; } // Change to int (not nullable for primary key)
        public bool? IsAccepted { get; set; }
        public string? BookIsbn { get; set; }
        public int? UserId { get; set; }
        public string? BookTitle { get; set; }
        public int Price { get; set; }
        public string UserName { get; set; }


    }
}
