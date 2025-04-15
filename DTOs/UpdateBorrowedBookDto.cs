namespace OnlineLibrary.DTOs
{
    public class UpdateBorrowedBookDto
    {
        public string Dateofreturn { get; set; }

        public int Ordernumber { get; set; }

        public bool? Isaccepted { get; set; }
    }
}
