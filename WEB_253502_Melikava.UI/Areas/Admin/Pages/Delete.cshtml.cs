using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB_253502_Melikava.Domain.Entities;
using WEB_253502_Melikava.UI.Services.BookService;

namespace WEB_253502_Melikava.UI.Areas.Admin.Pages
{
    [Authorize(Policy ="admin")]
    public class DeleteModel : PageModel
    {
        private readonly IBookService _bookService;

        public DeleteModel(IBookService bookService)
        {
            _bookService = bookService;
        }

        [BindProperty]
        public Book Book { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id) //???
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _bookService.GetProductByIdAsync(id);

            if (book == null)
            {
                return NotFound();
            }
            else
            {
                Book = book.Data;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            await _bookService.DeleteProductAsync(id);
            return RedirectToPage("./Index");
        }
    }
}
