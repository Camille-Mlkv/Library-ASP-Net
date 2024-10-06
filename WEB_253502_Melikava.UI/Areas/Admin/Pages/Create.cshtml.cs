using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEB_253502_Melikava.Domain.Entities;
using WEB_253502_Melikava.UI.Services.BookService;

namespace WEB_253502_Melikava.UI.Areas.Admin.Pages
{
    public class CreateModel : PageModel
    {
        private readonly IBookService _bookService;

        public CreateModel(IBookService bookService)
        {
            _bookService = bookService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Book Book { get; set; } = default!;
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // _bookService.CreateProductAsync(Book, "dd");
            return RedirectToPage("./Index");
        }
    }
}
