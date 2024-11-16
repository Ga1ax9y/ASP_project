using System.Text.Json;
using System.Text;
using WEB_253505_Stanishewski.Domain.Models;
using WEB_253505_Stanishewski.UI.Services.GameService;
using WEB_253505_Stanishewski.Domain.Entities;
using WEB_253505_Stanishewski.UI.Services.FileService;

namespace WEB_253505_Stanishewski.UI.APIConnection.Services
{
    public class ApiGameService : IGameService
    {
        private readonly HttpClient _httpClient;
        private readonly string _pageSize;
        private readonly JsonSerializerOptions _serializerOptions;
        private readonly ILogger<ApiGameService> _logger;
        private readonly IFileService _fileService;
        public ApiGameService(HttpClient httpClient,IConfiguration configuration,
                                    ILogger<ApiGameService> logger, IFileService fileService)
        {
            _httpClient = httpClient;
            _pageSize = configuration.GetSection("ItemsPerPage").Value;
            _serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _logger = logger;
            _fileService = fileService;
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
            product.Image = "Images/noimage.jpg";
            if (formFile != null)
            {
                var imageUrl = await _fileService.SaveFileAsync(formFile);
                if (!string.IsNullOrEmpty(imageUrl))
                {
                    product.Image = imageUrl; // Установить URL загруженного изображения
                }
            }
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
        public async Task<ResponseData<Game>> GetProductByIdAsync(int id)
        {
            var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + $"games/{id}");
            var response = await _httpClient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<ResponseData<Game>>(_serializerOptions);
                return data;
            }
            _logger.LogError($"-----> Продукт не найден. Error: {response.StatusCode}");
            return ResponseData<Game>.Error($"Продукт не найден. Error: {response.StatusCode}");
        }
        public async Task UpdateProductAsync(int id, Game game, IFormFile? formFile)
        {
            if (formFile != null)
            {
                var imageUrl = await _fileService.SaveFileAsync(formFile);
                if (!string.IsNullOrEmpty(imageUrl))
                {
                    if (!string.IsNullOrEmpty(game.Image) && game.Image != "Images/noimage.jpg")
                    {
                        await _fileService.DeleteFileAsync(game.Image);
                    }
                    game.Image = imageUrl;
                }
            }
            // Формирование URI для запроса обновления
            var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + $"games/{id}");
            // Отправка запроса на обновление объекта
            var response = await _httpClient.PutAsJsonAsync(uri, game, _serializerOptions);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"-----> Объект не обновлен. Error: {response.StatusCode}");
                throw new InvalidOperationException($"Объект не обновлен. Error: {response.StatusCode}");
            }
        }
        public async Task DeleteProductAsync(int id)
        {
                var product = await GetProductByIdAsync(id);
                var uri = new Uri($"{_httpClient.BaseAddress.AbsoluteUri}games/{id}");
                var response = await _httpClient.DeleteAsync(uri);

                Game data = product.Data;
                await _fileService.DeleteFileAsync(data.Image);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"-----> object not deleted. Error: {response.StatusCode}");
                    throw new InvalidOperationException($"Объект не удален. Error: {response.StatusCode}");
                }
            }

    }
}


