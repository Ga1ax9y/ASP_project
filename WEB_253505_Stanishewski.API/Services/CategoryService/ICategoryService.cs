using WEB_253505_Stanishewski.Domain.Entities;
using WEB_253505_Stanishewski.Domain.Models;

namespace WEB_253505_Stanishewski.API.Services.CategoryService
{
    public interface IGameService
    {
        Task<ResponseData<ListModel<Game>>> GetProductListAsync(
        string? categoryNormalizedName, int pageNo = 1, int pageSize = 3);
        Task<ResponseData<Game>> GetProductByIdAsync(int id);
        Task UpdateProductAsync(int id, Game product);
        Task DeleteProductAsync(int id);
        Task<ResponseData<Game>> CreateProductAsync(Game product);
        Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile);
    }
}
