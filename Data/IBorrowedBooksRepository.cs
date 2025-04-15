namespace OnlineLibrary.Data
{
    public interface IBorrowedBooksRepository
    {
        Task<int> GetBorrowedBookAsync();
    }
}
