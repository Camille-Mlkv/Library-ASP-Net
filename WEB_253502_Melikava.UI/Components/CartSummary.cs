using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using WEB_253502_Melikava.UI.Models;

namespace WEB_253502_Melikava.UI.Components
{
    public class CartSummary:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var cartInfo = new Cart
            {
                TotalPrice = 10.0m,
                ItemsCount = 20  
            };

            return View(cartInfo); 
        }
    }

}
