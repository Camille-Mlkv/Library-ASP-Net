using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WEB_253502_Melikava.Domain.Entities;
using WEB_253502_Melikava.Domain.Models;
using WEB_253502_Melikava.UI.Extensions;
using WEB_253502_Melikava.UI.Services.BookService;

namespace WEB_253502_Melikava.UI.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly IBookService _bookService;
        private readonly Cart _cart;
        public CartController(IBookService bookService,Cart cart)
        {
            _bookService = bookService;
            _cart = cart;
        }

        [Route("add/{id:int}")]
        public async Task<IActionResult> Add(int id, string returnUrl)
        {
            var data = await _bookService.GetProductByIdAsync(id);
            if (data.Successfull)
            {
                _cart.AddToCart(data.Data);
            }

            return Redirect(returnUrl);
        }

        [Route("remove/{id:int}")]
        public IActionResult Remove(int id, string returnUrl)
        {
            _cart.RemoveItems(id);
            return Redirect(returnUrl);
        }

        [HttpPost]
        [Route("clear")]
        public IActionResult Clear()
        {
            _cart.ClearAll();
            return RedirectToAction("Index");
        }

        public IActionResult Index()
        {
            return View(_cart);
        }
    }
}
