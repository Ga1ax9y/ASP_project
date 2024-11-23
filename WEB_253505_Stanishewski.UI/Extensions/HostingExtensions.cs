using WEB_253505_Stanishewski.UI.APIConnection.Services;
using WEB_253505_Stanishewski.UI.APIConnection;
using WEB_253505_Stanishewski.UI.Services.CategoryService;
using WEB_253505_Stanishewski.UI.Services.GameService;
using WEB_253505_Stanishewski.UI.Services.FileService;
using WEB_253505_Stanishewski.UI.HelperClasses;
using WEB_253505_Stanishewski.UI.Services.Authentication;
using WEB_253505_Stanishewski.UI.Services.Authorization;

namespace WEB_253505_Stanishewski.UI.Extensions
{
    public static class HostingExtensions
    {
        public static void RegisterCustomServices(
        this WebApplicationBuilder builder, UriData? uriData)
        {
            builder.Services.AddScoped<ICategoryService, MemoryCategoryService>();
            builder.Services.AddScoped<IGameService, MemoryGameService>();
            builder.Services.AddHttpClient<IGameService, ApiGameService>(opt =>opt.BaseAddress = new Uri(uriData.ApiUri));
            builder.Services.AddHttpClient<ICategoryService, ApiCategoryService>(opt =>opt.BaseAddress = new Uri(uriData.ApiUri));
            builder.Services.AddHttpClient<IFileService, ApiFileService>(opt =>opt.BaseAddress = new Uri($"{uriData.ApiUri}Files"));
            builder.Services.Configure<KeycloakData>(builder.Configuration.GetSection("Keycloak"));
            builder.Services.AddHttpClient<ITokenAccessor, KeycloakTokenAccessor>();
            builder.Services.AddHttpClient<IAuthService, KeycloakAuthService>();
            builder.Services.AddHttpContextAccessor();
        }
    }

}
