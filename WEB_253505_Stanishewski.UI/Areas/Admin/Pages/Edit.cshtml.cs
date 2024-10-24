using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WEB_253505_Stanishewski.Domain.Entities;
using WEB_253505_Stanishewski.UI.Data;
using WEB_253505_Stanishewski.UI.Services.CategoryService;
using WEB_253505_Stanishewski.UI.Services.GameService;

namespace WEB_253505_Stanishewski.UI.Areas.Admin.Pages
{
    public class EditModel : PageModel
    {
        private readonly IGameService _gameService;
        private readonly ICategoryService _categoryService;

        public EditModel(IGameService gameService, ICategoryService categoryService)
        {
            _gameService = gameService;
            _categoryService = categoryService;
        }

        [BindProperty]
        public IFormFile? Image { get; set; }
        public SelectList Categories { get; set; }

        [BindProperty]
        public Game Game { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Загрузка блюда по ID
            var response = await _gameService.GetProductByIdAsync(id.Value);
            if (!response.Successfull || response.Data == null)
            {
                return NotFound(response.ErrorMessage);
            }

            Game = response.Data;

            // Загрузка списка категорий
            var categoryResponse = await _categoryService.GetCategoryListAsync();
            if (!categoryResponse.Successfull)
            {
                return NotFound(categoryResponse.ErrorMessage);
            }

            // Привязка категорий к SelectList
            Categories = new SelectList(categoryResponse.Data, "Id", "Name", Game.CategoryId);

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var existingDishResponse = await _gameService.GetProductByIdAsync(id);
            if (!existingDishResponse.Successfull || existingDishResponse.Data == null)
            {
                return NotFound(existingDishResponse.ErrorMessage);
            }
            var existingDish = existingDishResponse.Data;
            Game.Image = existingDish.Image;
            if (Image != null)
            {
                await _gameService.UpdateProductAsync(id, Game, Image);
            }
            else
            {
                await _gameService.UpdateProductAsync(id, Game, null);
            }
            return RedirectToPage("./Index");
        }
    }
}
