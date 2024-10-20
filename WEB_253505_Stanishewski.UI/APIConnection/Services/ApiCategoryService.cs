using System.Text.Json;
using WEB_253505_Stanishewski.Domain.Entities;
using WEB_253505_Stanishewski.Domain.Models;
using WEB_253505_Stanishewski.UI.Services.CategoryService;

namespace WEB_253505_Stanishewski.UI.APIConnection.Services
{
    public class ApiCategoryService : ICategoryService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _serializerOptions;
        private readonly ILogger<ApiCategoryService> _logger;
        public ApiCategoryService(HttpClient httpClient, ILogger<ApiCategoryService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _serializerOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        }
        public async Task<ResponseData<List<Category>>> GetCategoryListAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("categories");
                if (response.IsSuccessStatusCode)
                {
                    var categories = await response.Content.ReadFromJsonAsync<ResponseData<List<Category>>>(_serializerOptions);
                    return categories!;
                }
                else
                {
                    _logger.LogError($"Ошибка при получении категорий: {response.StatusCode}");
                    return ResponseData<List<Category>>.Error($"Ошибка при получении категорий: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Произошла ошибка: {ex.Message}");
                return ResponseData<List<Category>>.Error($"Произошла ошибка: {ex.Message}");
            }

        }
    }
}
