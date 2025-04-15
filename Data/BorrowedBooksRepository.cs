using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OnlineLibrary.Models;

namespace OnlineLibrary.Data
{
    public class BorrowedBooksRepository : Repository<BorrowedBook>,IBorrowedBooksRepository
    {
        

        public BorrowedBooksRepository(OnlineLibraryContext context) :base(context)
        {
 
        }
        public async Task<int> GetBorrowedBookAsync()
        {
            var count = await _context.BorrowedBooks
            .Where(b => b.Isaccepted == true)
            .CountAsync();
            return count;
        }
    }
}
