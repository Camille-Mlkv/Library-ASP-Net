using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEB_253502_Melikava.UI.Models;
using WEB_253502_Melikava.UI.Models.ViewModels;

namespace WEB_253502_Melikava.UI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // Create a list of items
            var items = new List<ListDemo>
            {
                new ListDemo { Id = "1", Name = "Item 1" },
                new ListDemo { Id = "2", Name = "Item 2" },
                new ListDemo { Id = "3", Name = "Item 3" }
            };
            var model = new HomeViewModel();
            model.ItemsList = new List<SelectListItem>();

            foreach (var item in items)
            {
                model.ItemsList.Add(new SelectListItem { Text = item.Name, Value = item.Id });
            }
           
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(HomeViewModel model)
        {
            var selectedItem = model.SelectedId;
            return RedirectToAction("Index");
        }
    }
}
