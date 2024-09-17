using Microsoft.AspNetCore.Mvc;

namespace WEB_253505_Stanishewski.UI.Components
{
    public class CartViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var cartInfo = await GetCartInfoAsync();

            return View(cartInfo);
        }

        private Task<Cart> GetCartInfoAsync()
        {
            return Task.FromResult(new Cart
            {
                ItemCount = 0,
                TotalPrice = 0
            });
        }
    }

    public class Cart
    {
        public int ItemCount { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
