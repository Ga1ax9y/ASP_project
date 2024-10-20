using System.Text.Json;
using System.Text;
using WEB_253505_Stanishewski.Domain.Models;
using WEB_253505_Stanishewski.UI.Services.GameService;
using WEB_253505_Stanishewski.Domain.Entities;

namespace WEB_253505_Stanishewski.UI.APIConnection.Services
{
    public class ApiGameService : IGameService
    {
        private readonly HttpClient _httpClient;
        private readonly string _pageSize;
        private readonly JsonSerializerOptions _serializerOptions;
        private readonly ILogger<ApiGameService> _logger;
        public ApiGameService(HttpClient httpClient,
         IConfiguration configuration,
         ILogger<ApiGameService> logger)
        {
            _httpClient = httpClient;
            _pageSize = configuration.GetSection("ItemsPerPage").Value;
            _serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _logger = logger;
        }
        public async Task<ResponseData<ListModel<Game>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
        {
            // подготовка URL запроса
            var urlString
            = new
             StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}games");
            // добавить категорию в маршрут
            if (categoryNormalizedName != null)
            {
                urlString.Append($"/category/{categoryNormalizedName}/");
            };
            var a = new Uri(urlString.ToString());
            // добавить номер страницы в маршрут
            if (pageNo > 1)
            {
                urlString.Append($"?pageNo={pageNo}");
            };
            // добавить размер страницы в строку запроса
            if (!_pageSize.Equals("3"))
            {
                if (pageNo > 1)
                    urlString.Append($"&pageSize={_pageSize}");
                else
                    urlString.Append($"?pageSize={_pageSize}");
            }

            // отправить запрос к API
            var response = await _httpClient.GetAsync(
            new Uri(urlString.ToString()));

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content.ReadFromJsonAsync<ResponseData<ListModel<Game>>>(_serializerOptions);
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"-----> Ошибка: {ex.Message}");
                    return ResponseData<ListModel<Game>>
                    .Error($"Ошибка: {ex.Message}");
                }
            }
            _logger.LogError($"-----> Данные не получены от сервера. Error: {response.StatusCode.ToString()}");
            return ResponseData<ListModel<Game>>
                   .Error($"Данные не получены от сервера. Error: {response.StatusCode.ToString()}");
        }
        public async Task<ResponseData<Game>> CreateProductAsync(Game product,IFormFile? formFile)
        {
            var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + "Games");

            var response = await _httpClient.PostAsJsonAsync(
            uri,
            product,
            _serializerOptions);
            if (response.IsSuccessStatusCode)
            {
                var data = await response
                    .Content
                    .ReadFromJsonAsync<ResponseData<Game>>(_serializerOptions);

                return data; 
            }
            _logger
            .LogError($"-----> object not created. Error: { response.StatusCode.ToString()}");
            return ResponseData<Game>.Error($"Объект не добавлен. Error: {response.StatusCode.ToString()}");
        }
        public Task<ResponseData<Game>> GetProductByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
        public Task UpdateProductAsync(int id, Game game, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }
        public Task DeleteProductAsync(int id)
        {
            throw new NotImplementedException();
        }

    }
}


