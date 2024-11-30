using System.ComponentModel.DataAnnotations;

namespace WEB_253502_Melikava.Blazor.SSR.Models
{
    public class NumberModel
    {
        [Required]
        [Range(1, 10)]
        public int Quantity { get; set; }
    }
}
