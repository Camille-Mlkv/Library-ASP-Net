using Microsoft.AspNetCore.Mvc;

namespace WEB_253502_Melikava.UI.Models.ViewModels
{
    public class DemoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
