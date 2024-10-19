using Microsoft.EntityFrameworkCore;
using WEB_253505_Stanishewski.Domain.Entities;

namespace WEB_253505_Stanishewski.API.Data
{
    public static class DbInitializer
    {
        public static async Task SeedData(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            if (!context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category {Name="Экшен", NormalizedName="action"},
                    new Category {Name="Приключения", NormalizedName="adventure"},
                    new Category {Name="Ролевые", NormalizedName="role-playing"},
                    new Category {Name="Стратегии", NormalizedName="strategy"},
                    new Category {Name="Спортивные", NormalizedName="sports"},
                    new Category {Name="Файтинг", NormalizedName="fighting"}
                };
                await context.Categories.AddRangeAsync(categories);
                await context.SaveChangesAsync();
            }
            var baseUrl = app.Configuration.GetValue<string>("ApplicationUrl");
            if (!context.Games.Any())
            {
                var categories = await context.Categories.ToListAsync();
                var games = new List<Game>
                {
                    new Game { Title="Elden ring",Description="Хорошая игра",Price=50, Image=$"{baseUrl}/Images//Elden_Ring.jpg",Category=categories.Find(c=>c.NormalizedName.Equals("role-playing")), CategoryId = categories.Find(c => c.NormalizedName.Equals("role-playing"))?.Id ?? 0},
                    new Game { Title="Wither 3",Description="Очень Крутая игра",Price=50, Image=$"{baseUrl}/Images//The_Witcher_3.jpg",Category=categories.Find(c=>c.NormalizedName.Equals("role-playing")), CategoryId = categories.Find(c => c.NormalizedName.Equals("role-playing")) ?.Id ?? 0},
                    new Game { Title="Resident Evil 2",Description="Очень Крутая игра",Price=40, Image=$"{baseUrl}/Images//Resident_Evil_2.jpg",Category=categories.Find(c=>c.NormalizedName.Equals("adventure")),CategoryId = categories.Find(c => c.NormalizedName.Equals("adventure"))?.Id ?? 0},
                    new Game { Title="EA FC 25",Description="НЕОчень Крутая игра",Price=75, Image=$"{baseUrl}/Images//EA_FC_25.jpg",Category=categories.Find(c=>c.NormalizedName.Equals("sports")),CategoryId=categories.Find(c => c.NormalizedName.Equals("sports"))?.Id ?? 0},
                    new Game { Title="Red Dead Redemption 2",Description="Супер игра",Price=80, Image=$"{baseUrl}/Images//Red_Dead_Redemption.jpg",Category=categories.Find(c=>c.NormalizedName.Equals("role-playing")), CategoryId = categories.Find(c => c.NormalizedName.Equals("role-playing"))?.Id ?? 0},
                    new Game { Title="God of War 4",Description="Очень Крутая игра",Price=60, Image=$"{baseUrl}/Images//God_of_War_2018.jpg",Category=categories.Find(c=>c.NormalizedName.Equals("adventure")), CategoryId = categories.Find(c => c.NormalizedName.Equals("adventure")) ?.Id ?? 0},
                    new Game { Title="Mortal Kombat 1",Description="Очень Крутая игра",Price=30, Image=$"{baseUrl}/Images//Mortal_Kombat_1.jpeg",Category=categories.Find(c=>c.NormalizedName.Equals("fighting")),CategoryId = categories.Find(c => c.NormalizedName.Equals("fighting"))?.Id ?? 0},
                    new Game { Title="NHL 24",Description="Очень Крутая игра",Price=35, Image=$"{baseUrl}/Images//nhl24.jpg",Category=categories.Find(c=>c.NormalizedName.Equals("sports")),CategoryId=categories.Find(c => c.NormalizedName.Equals("sports"))?.Id ?? 0},

                };
                await context.Games.AddRangeAsync(games);
                await context.SaveChangesAsync();
            }

        }
    }
}
