
using Microsoft.EntityFrameworkCore;
using WEB_253505_Stanishewski.API.Data;
using WEB_253505_Stanishewski.API.Services.CategoryService;
using WEB_253505_Stanishewski.API.Services.GameService;
using WEB_253505_Stanishewski.Domain.Entities;

namespace WEB_253505_Stanishewski.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            string connectionString = builder.Configuration.GetConnectionString("Default");  
            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionString));

            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IGameService, GameService>();
            var app = builder.Build();
            
            await DbInitializer.SeedData(app);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseStaticFiles();
            app.MapControllers();

            app.Run();
        }
    }
}
