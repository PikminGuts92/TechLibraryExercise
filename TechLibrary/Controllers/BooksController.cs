using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using TechLibrary.Domain;
using TechLibrary.Models;
using TechLibrary.Services;

namespace TechLibrary.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BooksController : Controller
    {
        private readonly ILogger<BooksController> _logger;
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;

        public BooksController(ILogger<BooksController> logger, IBookService bookService, IMapper mapper)
        {
            _logger = logger;
            _bookService = bookService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Get all books");

            var books = await _bookService.GetBooksAsync();

            var bookResponse = _mapper.Map<List<BookResponse>>(books);

            return Ok(bookResponse);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation($"Get book by id {id}");

            var book = await _bookService.GetBookByIdAsync(id);

            var bookResponse = _mapper.Map<BookResponse>(book);

            return Ok(bookResponse);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> UpdateById(int id, [FromBody] BookResponse book)
        {
            _logger.LogInformation($"Update book by id {id}");
            book.BookId = id;

            var bookDomain = _mapper.Map<Book>(book);
            var updatedBook = await _bookService.UpdateBookAsync(bookDomain);

            var bookContract = _mapper.Map<BookResponse>(updatedBook);
            return Ok(bookContract);
        }

        [HttpPost("search")]
        public async Task<IActionResult> SearchBooks([FromBody] SearchRequest bookReq)
        {
            _logger.LogInformation($"Searching books (Term: \"{bookReq.Text}\", Page: {bookReq.Page}, Page Size: {bookReq.ItemsPerPage})");

            var bookServiceResp = await _bookService.SearchBooksAsync(bookReq);

            var response = _mapper.Map<SearchResponse<BookResponse>>(bookServiceResp);

            return Ok(response);
        }
    }
}
