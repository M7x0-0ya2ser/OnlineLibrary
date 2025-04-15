using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using OnlineLibrary.Data;
using OnlineLibrary.DTOs;
using OnlineLibrary.Models;

namespace OnlineLibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowedBooksController : ControllerBase
    {
        private readonly IDataRepository<BorrowedBook> _borrowedbooksrepo;
        private readonly IDataRepository<User> _userrepo;
        private readonly IDataRepository<Book> _bookrepo;
        private readonly IBorrowedBooksRepository _borrowbookrepo;
        private readonly IMapper _mapper;
        private static Random random = new Random();

        public BorrowedBooksController(IDataRepository<BorrowedBook> repo,
            IMapper mapper,
            IDataRepository<User> userrepo,
            IDataRepository<Book> bookrepo ,IBorrowedBooksRepository borrowedBooksRepository
            )
        {

            _borrowedbooksrepo = repo;
            _mapper = mapper;
            _userrepo = userrepo;
            _bookrepo = bookrepo;
            _borrowbookrepo = borrowedBooksRepository;
        }

        [HttpGet]

        public async Task<IActionResult> GetBorrowedBooks()
        {
            var returnedBooks = await _borrowedbooksrepo.GetQueryable()
            .Join(
            _bookrepo.GetQueryable(), // No lambda needed here (automatic include)
                borrowedBook => borrowedBook.Bookisbn,
                book => book.Isbn,
                (borrowedBook, book) => new BorrowedBookDto
                {
                    DateOfReturn = borrowedBook.Dateofreturn,
                    OrderNumber = (int)borrowedBook.Ordernumber,
                    IsAccepted = borrowedBook.Isaccepted,
                    BookIsbn = borrowedBook.Bookisbn,
                    UserId = borrowedBook.Userid,
                    BookTitle = book.Title,
                    Price = book.Price,
                    UserName = null
                }
            )
            .Join(
                _userrepo.GetQueryable(),
                borrowedBookDto => borrowedBookDto.UserId,
                user => user.Id,
                (borrowedBookDto, user) =>
                  new BorrowedBookDto
                  {
                      DateOfReturn = borrowedBookDto.DateOfReturn,
                      OrderNumber = borrowedBookDto.OrderNumber,
                      IsAccepted = borrowedBookDto.IsAccepted,
                      BookIsbn = borrowedBookDto.BookIsbn,
                      UserId = borrowedBookDto.UserId,
                      BookTitle = borrowedBookDto.BookTitle,
                      Price = borrowedBookDto.Price,
                      UserName = user.Username
                  }
              )
              .ToListAsync();

            return Ok(returnedBooks);
        }



        [HttpGet("{id}"), AllowAnonymous]

        public async Task<IActionResult> GetBorrowedBookByID(int id)
        {
            var returnedBooks = await _borrowedbooksrepo.GetQueryable().Where(u => u.Userid == id)
      .Join(
        _bookrepo.GetQueryable(), // No lambda needed here (automatic include)
        borrowedBook => borrowedBook.Bookisbn,
        book => book.Isbn,
        (borrowedBook, book) => new BorrowedBookDto
        {
            DateOfReturn = borrowedBook.Dateofreturn,
            OrderNumber = (int)borrowedBook.Ordernumber,
            IsAccepted = borrowedBook.Isaccepted,
            BookIsbn = borrowedBook.Bookisbn,
            UserId = borrowedBook.Userid,
            BookTitle = book.Title,
            Price = book.Price,
            UserName = null // Assuming you'll populate this later
        }
      )
      .Join(
        _userrepo.GetQueryable(),
        borrowedBookDto => borrowedBookDto.UserId,
        user => user.Id,
        (borrowedBookDto, user) =>  // Simplified lambda
          new BorrowedBookDto // Create a new instance
          {
              DateOfReturn = borrowedBookDto.DateOfReturn,
              OrderNumber = borrowedBookDto.OrderNumber,
              IsAccepted = borrowedBookDto.IsAccepted,
              BookIsbn = borrowedBookDto.BookIsbn,
              UserId = borrowedBookDto.UserId,
              BookTitle = borrowedBookDto.BookTitle,
              Price = borrowedBookDto.Price,
              UserName = user.Username
          }
      )
      .ToListAsync();



            if (returnedBooks == null)
            {
                return NotFound("There is no record for this user");
            }

            return Ok(returnedBooks);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBorrowedBook(int id)
        {
            var borrowedBook = await _borrowedbooksrepo.GetByIdAsync(id);

            if (borrowedBook == null)
            {
                return NotFound("There are no borrowed books for this user");
            }

            _borrowedbooksrepo.Delete(borrowedBook);
            await _borrowedbooksrepo.SaveChangesAsync();

            return Ok();
        }




        [HttpPost]

        public async Task<IActionResult> AddBorrowedBook(ISBNdto dto)
        {
            var book = _mapper.Map<BorrowedBook>(dto);

            book.Isaccepted = null;
            book.Dateofreturn = null;
            book.Ordernumber = GetRandom(10, 10000);
            _borrowedbooksrepo.Insert(book);
            await _borrowedbooksrepo.SaveChangesAsync();
            return Ok();

        }
        [HttpPut]

        public async Task<IActionResult> UpdateBorrowedBook(UpdateBorrowedBookDto dto)
        {
            var book = await _borrowedbooksrepo.GetByIdAsync(dto.Ordernumber);
            if (dto.Isaccepted == true)
            {
                book.Dateofreturn = DateOnly.Parse(dto.Dateofreturn);
            }
            book.Isaccepted = dto.Isaccepted;
            _borrowedbooksrepo.Update(book);
            await _borrowedbooksrepo.SaveChangesAsync();
            return Ok();

        }



        private int GetRandom(int min, int max)
        {
            return random.Next(min, max);
        }

        //[HttpGet]

        //public async Task<IActionResult> GenerateReports()
        //{

        //}

        [HttpGet("/GetCountBorrowedBook"), Authorize(Roles = "Admin")]

        public async Task<IActionResult> GetCountBorrowedBook()
        {
            var check = await _borrowbookrepo.GetBorrowedBookAsync();
            return Ok(check);
        }


        [HttpGet("/GetAvailableBook"), Authorize(Roles = "Admin")]

        public async Task<IActionResult> GetGetAvailableBook()
        {
            return Ok(await _bookrepo.GetAllAsync(b => b.Stocknumber > 0));
        }



    } 
}

