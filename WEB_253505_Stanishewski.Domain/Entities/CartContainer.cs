using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEB_253505_Stanishewski.Domain.Entities
{
    public class CartContainer
    {
        public Dictionary<int, CartItem> CartItems { get; set; } = new();
        public virtual void AddToCart(Game game)
        {
            if (game == null)
                throw new ArgumentNullException(nameof(game));
            int id = game.Id;
            if (CartItems.ContainsKey(id))
            {
                CartItems[id].Amount++;
            }
            else
            {
                CartItems[id] = new CartItem { Amount = 1, Item = game };
            }
        }
        public virtual void RemoveItems(int id)
        {
            if (CartItems.ContainsKey(id))
            {
                var cartItem = CartItems[id];
                if (cartItem.Amount > 1)
                {
                    cartItem.Amount--;
                }
                else
                {
                    CartItems.Remove(id);
                }
            }
        }
        public virtual void ClearAll()
        {
            CartItems.Clear();
        }
        public int Count { get => CartItems.Sum(item => item.Value.Amount); }
        public double TotalPrice { get => (double)CartItems.Sum(item => item.Value.Item.Price * item.Value.Amount); }
    }
}
