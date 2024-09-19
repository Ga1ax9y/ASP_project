using WEB_253505_Stanishewski.Domain.Entities;
using WEB_253505_Stanishewski.UI.Services.CategoryService;

namespace WEB_253505_Stanishewski.UI.Services.GameService
{
    public class MemoryProductService : IGameService
    {
        List<Game> _games;
        List<Category> _categories;
        public MemoryProductService(ICategoryService categoryService)
        {
            _categories = categoryService.GetCategoryListAsync().Result.Data;
            SetupData();
        }

        private void SetupData()
        {
            _games = new List<Game>
            {
                new Game { Id = 1, Title="Elden ring",
                Description="Крутая игра",
                ca
                }
            }

    }


    }

}
