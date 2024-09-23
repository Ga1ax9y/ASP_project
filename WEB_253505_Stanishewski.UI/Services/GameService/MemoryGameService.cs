using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WEB_253505_Stanishewski.Domain.Entities;
using WEB_253505_Stanishewski.Domain.Models;
using WEB_253505_Stanishewski.UI.Services.CategoryService;

namespace WEB_253505_Stanishewski.UI.Services.GameService
{
    public class MemoryGameService : IGameService
    {
        private readonly IConfiguration _config;
        List<Game> _games;
        List<Category> _categories;
        public MemoryGameService([FromServices] IConfiguration config, ICategoryService categoryService)
        {
            _categories = categoryService.GetCategoryListAsync().Result.Data;
            SetupData();
            _config = config;
        }

        private void SetupData()
        {
            _games = new List<Game>
            {
                new Game { Id = 1, Title="Elden ring",
                Description="Крутая игра",
                Price=50, Image="Images/Elden_Ring.jpg",
                Category=_categories.Find(c=>c.NormalizedName.Equals("role-playing"))
                },
                new Game { Id = 2, Title="Wither 3",
                Description="Очень Крутая игра",
                Price=50, Image="Images/The_Witcher_3.jpg",
                Category=_categories.Find(c=>c.NormalizedName.Equals("role-playing"))
                },
                new Game { Id = 3, Title="Resident Evil 2",
                Description="Очень Крутая игра",
                Price=40, Image="Images/Resident_Evil_2.jpg",
                Category=_categories.Find(c=>c.NormalizedName.Equals("adventure"))
                },
                new Game { Id = 4, Title="EA FC 25",
                Description="НЕОчень Крутая игра",
                Price=75, Image="Images/EA_FC_25.jpg",
                Category=_categories.Find(c=>c.NormalizedName.Equals("sports"))
                }
            };

        }
        public Task<ResponseData<ListModel<Game>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
        {
            if (pageNo < 1)
                return Task.FromResult(ResponseData<ListModel<Game>>.Error("Нет такой страницы"));
            var data = _games
            .Where(d => categoryNormalizedName == null || d.Category.NormalizedName.Equals(categoryNormalizedName))
            .ToList();
            int itemsPerPage = _config.GetValue<int>("ItemsPerPage");
            int totalItems = data.Count;
            int totalPages = (int)Math.Ceiling(totalItems / (double)itemsPerPage);
            var pagedItems = data
            .Skip((pageNo - 1) * itemsPerPage)
            .Take(itemsPerPage)
            .ToList();
            var result = new ListModel<Game>
            {
                Items = pagedItems,
                CurrentPage = pageNo,
                TotalPages = totalPages
            };

            return Task.FromResult(ResponseData<ListModel<Game>>.Success(result));
        }
        public Task<ResponseData<Game>> GetProductByIdAsync(int id)
        {
            return null;
        }
        public Task UpdateProductAsync(int id, Game game, IFormFile? formFile)
        {
            return Task.CompletedTask;
        }
        public Task DeleteProductAsync(int id)
        {
            return Task.CompletedTask;
        }
        public Task<ResponseData<Game>> CreateProductAsync(Game game, IFormFile? formFile)
        {
            return null;
        }




    }

}
