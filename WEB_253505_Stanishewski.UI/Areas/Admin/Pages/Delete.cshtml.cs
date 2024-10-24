using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WEB_253505_Stanishewski.Domain.Entities;
using WEB_253505_Stanishewski.UI.Data;
using WEB_253505_Stanishewski.UI.Services.GameService;

namespace WEB_253505_Stanishewski.UI.Areas.Admin.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly IGameService _gameService;

        public DeleteModel(IGameService gameService)
        {
            _gameService = gameService;
        }

        [BindProperty]
        public Game Game { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await _gameService.GetProductByIdAsync(id.Value);
            if (!response.Successfull || response.Data == null)
            {
                return NotFound(response.ErrorMessage);
            }

            Game = response.Data;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await _gameService.DeleteProductAsync(id.Value);
            return RedirectToPage("./Index");
        }
    }
}
