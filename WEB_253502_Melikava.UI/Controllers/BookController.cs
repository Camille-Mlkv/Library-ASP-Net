using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using WEB_253502_Melikava.Domain.Entities;
using WEB_253502_Melikava.Domain.Models;
using WEB_253502_Melikava.UI.Services.BookService;
using WEB_253502_Melikava.UI.Services.GenreService;
using WEB_253502_Melikava.UI.Extensions;

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
            var genresResponse = await _genreService.GetGenreListAsync();
            if (!genresResponse.Successfull)
            {
                return NotFound(genresResponse.ErrorMessage);
            }
            var genres = genresResponse.Data;
            var current_genre=genre==null? null: genres.FirstOrDefault(g=>g.NormalizedName==genre);
            ResponseData<BookListModel<Book>> bookResponse = new ResponseData<BookListModel<Book>>();


            ViewBag.Genres = genres;
            ViewBag.CurrentGenre = current_genre;
            bookResponse = await _bookService.GetProductListAsync(genre, pageNo);

            if (!bookResponse.Successfull)
            {
                return NotFound(bookResponse.ErrorMessage);
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("_CatalogAsync", bookResponse.Data);
            }
            return View(bookResponse.Data);
        }
    }
}
