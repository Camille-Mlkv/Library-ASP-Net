using Microsoft.AspNetCore.Mvc;
using WEB_253502_Melikava.Domain.Entities;
using WEB_253502_Melikava.Domain.Models;
using WEB_253502_Melikava.UI.Services.BookService;
using WEB_253502_Melikava.UI.Services.GenreService;

namespace WEB_253502_Melikava.UI.Controllers
{
    public class BookController : Controller
    {
        private IGenreService _genreService;
        private IBookService _bookService;
        public BookController(IGenreService genreService, IBookService bookService)
        {
            _genreService = genreService;
            _bookService=bookService;
        }
        public async Task<IActionResult> Index(string? genre, int pageNo = 1)//передаем normalized
        {
            var genres = (await _genreService.GetGenreListAsync()).Data;
            var current_genre=genres.FirstOrDefault(g=>g.NormalizedName==genre);
            ResponseData<BookListModel<Book>> bookResponse = new ResponseData<BookListModel<Book>>();


            ViewBag.Genres = genres;
            ViewBag.CurrentGenre = current_genre;
            bookResponse = await _bookService.GetProductListAsync(genre, pageNo);

            if (!bookResponse.Successfull)
            {
                return NotFound(bookResponse.ErrorMessage);
            }
            return View(bookResponse.Data);
        }
    }
}
