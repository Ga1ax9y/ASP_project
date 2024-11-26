
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using WEB_253505_Stanishewski.API.Data;
using WEB_253505_Stanishewski.API.Models;
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
            var authServer = builder.Configuration
    .GetSection("AuthServer")
    .Get<AuthServerData>();

            // Добавить сервис аутентификации 
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, o =>
                {
                    // Адрес метаданных конфигурации OpenID 
                    o.MetadataAddress = $"{authServer.Host}/realms/{authServer.Realm}/.well-known/openid-configuration"; 
            
                    // Authority сервера аутентификации 
                    o.Authority = $"{authServer.Host}/realms/{authServer.Realm}";

                    // Audience для токена JWT 
                    o.Audience = "account";

                    // Запретить HTTPS для использования локальной версии Keycloak 
                    // В рабочем проекте должно быть true 
                    o.RequireHttpsMetadata = false;
                });
            builder.Services.AddAuthorization(opt =>
            {
                opt.AddPolicy("admin", p => p.RequireRole("POWER-USER"));
            });
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });
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

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStaticFiles();
            app.UseCors("AllowAll");
            app.MapControllers();

            app.Run();
        }
    }
}
