using Microsoft.AspNetCore.Mvc;
using WEB_253505_Stanishewski.Domain.Entities;
using WEB_253505_Stanishewski.UI.Extensions;

namespace WEB_253505_Stanishewski.UI.ViewComponents
{
    public class CartViewComponent : ViewComponent
    {
        private CartContainer _cart;
        public CartViewComponent(CartContainer cart)
        {
            _cart = cart;
        }
        public IViewComponentResult Invoke()
        {
            // Извлечение корзины из сессии
            var cart = HttpContext.Session.Get<CartContainer>("cart") ?? new CartContainer();

            // Подсчет итогов
            var totalItems = cart.CartItems.Sum(item => item.Value.Amount);
            var totalPrice = cart.CartItems.Sum(item => item.Value.Amount * item.Value.Item.Price);

            // Формирование данных для отображения
            var cartInfo = new { TotalItems = totalItems, TotalPrice = totalPrice };

            // Возвращение представления
            return View(cartInfo);
        }
    }
}
