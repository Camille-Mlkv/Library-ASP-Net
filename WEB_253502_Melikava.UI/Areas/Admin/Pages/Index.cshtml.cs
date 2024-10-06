using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB_253502_Melikava.UI.Services.BookService;
using WEB_253502_Melikava.Domain.Entities;
using WEB_253502_Melikava.Domain.Models;
using System.Drawing.Printing;

namespace WEB_253502_Melikava.UI.Areas.Admin.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IBookService _bookService;

        public IndexModel(IBookService bookService)
        {
            _bookService = bookService;
        }

        public BookListModel<Book> Books { get;set; } = default!;
        public int CurrentPage { get; set; } = 1; // Текущая страница
        public int TotalPages { get; set; }

        public async Task OnGetAsync(int pageNumber,int pageSize)
        {
            //Books = (await _bookService.GetProductListAsync("", pageNumber)).Data;
            // Установка текущей страницы и размера страницы, если переданы параметры
            CurrentPage = pageNumber;
            int PageSize = 3;

            // Получаем данные книг, используя номер и размер страницы
            var result = await _bookService.GetProductListAsync("",CurrentPage);
            Books = result.Data;

            // Рассчитываем общее количество страниц
            TotalPages = Books.TotalPages;
        }
    }
}
