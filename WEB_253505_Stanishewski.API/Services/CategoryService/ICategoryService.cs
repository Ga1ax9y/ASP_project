using WEB_253505_Stanishewski.Domain.Entities;
using WEB_253505_Stanishewski.Domain.Models;

namespace WEB_253505_Stanishewski.API.Services.CategoryService
{
    public interface ICategoryService
    {
        public Task<ResponseData<List<Category>>> GetCategoryListAsync();
    }
}
