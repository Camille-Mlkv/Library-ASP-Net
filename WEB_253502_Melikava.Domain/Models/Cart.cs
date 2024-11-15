using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEB_253502_Melikava.Domain.Entities;

namespace WEB_253502_Melikava.Domain.Models
{
    public class Cart
    {
        /// <summary> 
        /// Список объектов в корзине 
        /// key - идентификатор объекта 
        /// </summary> 
        public Dictionary<int, CartItem> CartItems { get; set; } = new();
        /// <summary> 
        /// Добавить объект в корзину 
        /// </summary> 
        /// <param name="book">Добавляемый объект</param> 
        public virtual void AddToCart(Book book)
        { 
            if(CartItems.ContainsKey(book.Id))
            {
                CartItems[book.Id].Amount++;
            }
            else
            {
                CartItems[book.Id] = new CartItem() { Book=book,Amount=1};
            }
        }
        /// <summary> 
        /// Удалить объект из корзины 
        /// </summary> 
        /// <param name="id"> id удаляемого объекта</param> 
        public virtual void RemoveItems(int id)
        {
            if (CartItems.ContainsKey(id))
            {
                CartItems.Remove(id);
            }

        }
        /// <summary> 
        /// Очистить корзину 
        /// </summary> 
        public virtual void ClearAll()
        {
            CartItems.Clear();
        }
        /// <summary> 
        /// Количество объектов в корзине 
        /// </summary> 
        public int Count { get => CartItems.Sum(item => item.Value.Amount); }
        /// <summary> 
        /// Сумма стоимости книг 
        /// </summary> 
        public double TotalPrice
        {
            get =>CartItems.Sum(item => item.Value.Book.Price * item.Value.Amount);
        }
    }
}

