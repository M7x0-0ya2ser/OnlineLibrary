using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineLibrary.Data;
using OnlineLibrary.Models;

namespace OnlineLibrary.Controllers
{
    [Route("api/v1/books")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IDataRepository<Book> _bookRepository;

        public BookController(IDataRepository<Book> bookRepository)
        {
            _bookRepository = bookRepository;
        }


        [HttpGet]

        public async Task<IActionResult> GetAllBooks()
        {
            var books =await _bookRepository.GetAllAsync();

            return Ok(books);

        }

        [HttpGet("{isbn}")]

        public async Task<IActionResult> GetBookByIsbn(string isbn)
        {
            var book = await _bookRepository.GetByIdAsync(isbn);

            return Ok(book);
        }


        [HttpPost, Authorize(Roles = "Admin")]

        public async Task<IActionResult> AddBook(Book book)
        {
            _bookRepository.Insert(book);
            await _bookRepository.SaveChangesAsync();
            return Ok(book);
        }


       


        [HttpDelete, Authorize(Roles = "Admin")]

        public async Task<IActionResult> DeleteBook(string isbn)
        {
            var book = await _bookRepository.GetByIdAsync(isbn);

            _bookRepository.Delete(book);
            await _bookRepository.SaveChangesAsync();

            return Ok();
        }


        [HttpPut, Authorize(Roles = "Admin")]

        public async Task<IActionResult> EditBook(Book updatedbook)
        {
            var existingbook = await _bookRepository.GetByIdAsync(updatedbook.Isbn);
            existingbook.Isbn = updatedbook.Isbn;
            existingbook.Stocknumber = updatedbook.Stocknumber;
            existingbook.Racknumber = updatedbook.Racknumber;
            existingbook.Price = updatedbook.Price;
            existingbook.Title = updatedbook.Title;
            existingbook.Category = updatedbook.Category;
            _bookRepository.Update(existingbook);
            await _bookRepository.SaveChangesAsync();
            return Ok();
        }

        
    }
}
