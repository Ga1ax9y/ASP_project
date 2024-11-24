using Microsoft.AspNetCore.Mvc;
using WEB_253505_Stanishewski.Domain.Entities;
using WEB_253505_Stanishewski.UI.Extensions;
using WEB_253505_Stanishewski.UI.Services.CategoryService;
using WEB_253505_Stanishewski.UI.Services.GameService;

namespace WEB_253505_Stanishewski.UI.Controllers
{
    [Route("catalog")]
    public class ProductController : Controller
    {
        private readonly IGameService _gameService;
        private readonly ICategoryService _categoryService;
        public ProductController(IGameService gameService,ICategoryService categoryService)
        {
            _gameService = gameService;
            _categoryService = categoryService;
        }
        [HttpGet("{category?}")]
        public async Task<IActionResult> Index(string? category, int pageNo=1)
        {
            var categoriesResponse = await _categoryService.GetCategoryListAsync();
            if (!categoriesResponse.Successfull)
                return NotFound(categoriesResponse.ErrorMessage);

            var productResponse = await _gameService.GetProductListAsync(category, pageNo);
            if (!productResponse.Successfull)
                return NotFound(productResponse.ErrorMessage);
            var categories = categoriesResponse.Data;
            ViewData["currentCategory"] = categories.FirstOrDefault(c => c.NormalizedName == category)?.Name ?? "Все";
            ViewData["categories"] = categories;
            if (Request.IsAjaxRequest())
            {
                return PartialView("_GameListPartial", productResponse.Data);
            }
            return View(productResponse.Data);
        }
    }

    
}
