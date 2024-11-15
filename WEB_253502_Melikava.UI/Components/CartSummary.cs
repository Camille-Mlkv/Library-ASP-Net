using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using WEB_253502_Melikava.Domain.Models;
using WEB_253502_Melikava.UI.Extensions;
using WEB_253502_Melikava.UI.Models;

namespace WEB_253502_Melikava.UI.Components
{
    public class CartSummary:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            Cart cartInfo = HttpContext.Session.Get<Cart>("cart") ?? new();
            return View(cartInfo); 
        }
    }

}
