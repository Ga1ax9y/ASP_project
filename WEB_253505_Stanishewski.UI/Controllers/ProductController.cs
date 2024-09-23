using Microsoft.AspNetCore.Mvc;
using WEB_253505_Stanishewski.Domain.Entities;
using WEB_253505_Stanishewski.UI.Services.CategoryService;
using WEB_253505_Stanishewski.UI.Services.GameService;

namespace WEB_253505_Stanishewski.UI.Controllers
{
    public class ProductController : Controller
    {
        private readonly IGameService _gameService;
        private readonly ICategoryService _categoryService;
        public ProductController(ICategoryService categoryService, IGameService gameService)
        {
            _gameService = gameService;
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index(string? category, int pageNo=1)
        {
            var categoriesResponse = await _categoryService.GetCategoryListAsync();
            if (!categoriesResponse.Successfull)
                return NotFound(categoriesResponse.ErrorMessage);
            var categories = categoriesResponse.Data;
            ViewData["currentCategory"] = categories.FirstOrDefault(c => c.NormalizedName == category)?.Name ?? "Категории";
            ViewData["categories"] = categories;
            var productResponse = await _gameService.GetProductListAsync(category,pageNo);
            if (!productResponse.Successfull)
                return NotFound(productResponse.ErrorMessage);
            return View(productResponse.Data);
        }

    }
}
