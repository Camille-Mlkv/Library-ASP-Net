using Microsoft.AspNetCore.Mvc.Rendering;

namespace WEB_253502_Melikava.UI.Models.ViewModels
{
    public class HomeViewModel
    {
        public string SelectedId { get; set; }
        public List<SelectListItem> ItemsList { get; set; }
    }
}
