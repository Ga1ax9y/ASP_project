using WEB_253505_Stanishewski.Domain.Entities;

namespace WEB_253505_Stanishewski.BlazorWasm.Services
{
    public interface IDataService
    {
        event Action DataLoaded;
        List<Category> Categories { get; set; }
        List<Game> Games { get; set; }
        bool Success { get; set; }
        string ErrorMessage { get; set; }
        int TotalPages { get; set; }
        int CurrentPage { get; set; }
        Category SelectedCategory { get; set; }
        public Task GetProductListAsync(int pageNo = 1);
        public Task GetCategoryListAsync();
    }
}
