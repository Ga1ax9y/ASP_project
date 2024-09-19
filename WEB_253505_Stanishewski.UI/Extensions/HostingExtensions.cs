using WEB_253505_Stanishewski.UI.Services.CategoryService;
using WEB_253505_Stanishewski.UI.Services.GameService;

namespace WEB_253505_Stanishewski.UI.Extensions
{
    public static class HostingExtensions
    {
        public static void RegisterCustomServices(
        this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<ICategoryService, MemoryCategoryService>();
            builder.Services.AddScoped<IGameService, MemoryGameService>();
        }
    }

}
