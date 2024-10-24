using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEB_253505_Stanishewski.Domain.Entities;
using WEB_253505_Stanishewski.UI.Data;
using WEB_253505_Stanishewski.UI.Services.CategoryService;
using WEB_253505_Stanishewski.UI.Services.GameService;
using WEB_253505_Stanishewski.Domain.Models;

namespace WEB_253505_Stanishewski.UI.Areas.Admin.Pages
{
    public class CreateModel : PageModel
    {
        private readonly IGameService _gameService;
        private readonly ICategoryService _categoryService;

        public CreateModel(IGameService gameService, ICategoryService categoryService)
        {
            _gameService = gameService;
            _categoryService = categoryService;
        }
        [BindProperty]
        public IFormFile? Image { get; set; }

        [BindProperty]
        public Game Game { get; set; } = default!;
        public SelectList Categories { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var categoriesResponse = await _categoryService.GetCategoryListAsync();
            if (!categoriesResponse.Successfull)
                return NotFound(categoriesResponse.ErrorMessage);

            Categories = new SelectList(categoriesResponse.Data, "Id", "Name");
            return Page();
        }


        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var response = await _gameService.CreateProductAsync(Game, Image);
            if (!response.Successfull)
            {
                ModelState.AddModelError(string.Empty, response.ErrorMessage);
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
