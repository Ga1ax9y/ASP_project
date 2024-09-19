using WEB_253505_Stanishewski.Domain.Entities;
using WEB_253505_Stanishewski.Domain.Models;

namespace WEB_253505_Stanishewski.UI.Services.CategoryService
{
    public interface ICategoryService
    {
        /// <summary>
        /// Получение списка всех категорий
        /// </summary>
        /// <returns></returns>
        public Task<ResponseData<List<Category>>> GetCategoryListAsync();
    }

}
