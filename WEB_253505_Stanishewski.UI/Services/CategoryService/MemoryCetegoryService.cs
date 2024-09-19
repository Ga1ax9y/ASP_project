using WEB_253505_Stanishewski.Domain.Entities;
using WEB_253505_Stanishewski.Domain.Models;

namespace WEB_253505_Stanishewski.UI.Services.CategoryService
{
    public class MemoryCategoryService : ICategoryService
    {
        public Task<ResponseData<List<Category>>> GetCategoryListAsync()
        {
            var categories = new List<Category>
{
                new Category {Id=1, Name="Экшен", NormalizedName="action"},
                new Category {Id=2, Name="Приключения", NormalizedName="adventure"},
                new Category {Id=3, Name="Ролевые", NormalizedName="role-playing"},
                new Category {Id=4, Name="Стратегии", NormalizedName="strategy"},
                new Category {Id=5, Name="Спортивные", NormalizedName="sports"},
                new Category {Id=6, Name="Головоломки", NormalizedName="puzzle"}
};
            var result = ResponseData<List<Category>>.Success(categories);
            return Task.FromResult(result);
        }
    }
}
