using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WEB_253505_Stanishewski.Domain.Entities;
using WEB_253505_Stanishewski.UI.Extensions;
using WEB_253505_Stanishewski.UI.Services.GameService;

namespace WEB_253505_Stanishewski.UI.Controllers
{
    public class CartController : Controller
    {
        private readonly IGameService _productService;
        private CartContainer _cart;
        public CartController(IGameService productService, CartContainer cart)
        {
            _productService = productService;
            _cart = cart;
        }
        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
            CartContainer cart = HttpContext.Session.Get<CartContainer>("cart") ?? new();
            return View(cart);
        }
        [HttpPost]
        [Authorize]
        //[Route("[controller]/Add/{id:int}")]
        public async Task<ActionResult> Add(int id, string returnUrl)
        {
            var data = await _productService.GetProductByIdAsync(id);
            if (data.Successfull)
            {
                _cart.AddToCart(data.Data);
            }
            return Redirect(returnUrl);
        }
        [HttpPost]
        [Authorize]
        //[Route("[controller]/Add/{id:int}")]
        public async Task<ActionResult> Remove(int id, string returnUrl)
        {
            _cart.RemoveItems(id);
            return Redirect(returnUrl);
        }
        [HttpPost]
        [Authorize]
        //[Route("[controller]/Add/{id:int}")]
        public async Task<ActionResult> Clear(string returnUrl)
        {
            _cart.ClearAll();
            return Redirect(returnUrl);
        }

        [HttpPost]
        public IActionResult UpdateCartSummary()
        {
            return ViewComponent("Cart");
        }

    }
}
