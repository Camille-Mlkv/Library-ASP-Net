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
    public class EditModel : PageModel
    {
        private readonly IBookService _bookService;

        public EditModel(IBookService bookService)
        {
            _bookService = bookService;
        }

        [BindProperty]
        public Book Book { get; set; } = default!;

        [BindProperty]
        public IFormFile? NewImage { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book =  (await _bookService.GetProductByIdAsync(id)).Data;
            if (book == null)
            {
                return NotFound();
            }
            Book = book;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _bookService.UpdateProductAsync(id, Book, NewImage);
            return RedirectToPage("./Index");
        }

     
    }
}
