using WEB_253502_Melikava.Domain.Entities;
using WEB_253502_Melikava.Domain.Models;
using WEB_253502_Melikava.UI.Extensions;

namespace WEB_253502_Melikava.UI.Services
{
    public class SessionCart:Cart
    {
        private readonly ISession _session;

        public SessionCart(ISession session)
        {
            _session = session;
            var savedCart = _session.Get<Cart>("cart");
            if (savedCart != null && savedCart.CartItems.Any())
            {
                this.CartItems = savedCart.CartItems;
            }
        }

        public override void AddToCart(Book book)
        {
            base.AddToCart(book);
            _session.Set("cart", this);
        }

        public override void RemoveItems(int id)
        {
            base.RemoveItems(id);
            _session.Set("cart", this);
        }

        public override void ClearAll()
        {
            base.ClearAll();
            _session.Set("cart", this);
        }
    }
}
