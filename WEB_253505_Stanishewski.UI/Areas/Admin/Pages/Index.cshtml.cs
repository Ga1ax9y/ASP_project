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
    public class IndexModel : PageModel
    {
        private readonly IGameService _gameService;

        public IndexModel(IGameService gameService)
        {
            _gameService = gameService;
        }

        public IList<Game> Game { get; set; } = new List<Game>();
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; } = 1;

        public async Task OnGetAsync(int? pageNo)
        {
            CurrentPage = pageNo ?? 1;
            var response = await _gameService.GetProductListAsync(null, CurrentPage);
            if (response.Successfull)
            {
                Game = response.Data.Items;
                TotalPages = response.Data.TotalPages;
            }
        }
    }
}
