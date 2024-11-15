using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEB_253502_Melikava.Domain.Entities
{
    public class CartItem
    {
        public Book Book { get; set; }
        public int Amount { get; set; }
    }
}
