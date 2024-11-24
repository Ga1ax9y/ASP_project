using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEB_253505_Stanishewski.API.Data;
using WEB_253505_Stanishewski.API.Services.GameService;
using WEB_253505_Stanishewski.Domain.Entities;
using WEB_253505_Stanishewski.Domain.Models;

namespace WEB_253505_Stanishewski.Tests
{
    public class ProductServiceApiTests
    {
        private AppDbContext CreateContext()
        {
            // Создаем подключение к SQLite in-memory
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(connection)
                .Options;
            var context = new AppDbContext(options);
            context.Database.EnsureCreated();
            SeedData(context);
            return context;
        }
        private void SeedData(AppDbContext context)
        {
            var categories = new List<Category>
            {
            new Category { Id = 1, Name = "Strategy Games", NormalizedName = "strategy-games" },
            new Category { Id = 2, Name = "Adventure Games", NormalizedName = "adventure-games" },
            new Category { Id = 3, Name = "Action Games", NormalizedName = "action-games" },
            new Category { Id = 4, Name = "Simulation Games", NormalizedName = "simulation-games" },
            new Category { Id = 5, Name = "Puzzle Games", NormalizedName = "puzzle-games" },
            new Category { Id = 6, Name = "Party Games", NormalizedName = "party-games" }
            };
            context.Categories.AddRange(categories);
            var games = new List<Game>
            {
            new Game { Title = "Zombie Apocalypse", Description = "Survive against waves of undead monsters", Price = 200, Image = "", Category = null, CategoryId = 3 },
            new Game { Title = "Dragon Quest", Description = "Embark on a mythical journey to save the kingdom", Price = 330, Image = "", Category = null, CategoryId = 3 },
            new Game { Title = "Strategy Conquest", Description = "Build your army and conquer territories", Price = 250, Image = "", Category = null, CategoryId = 1 },
            new Game { Title = "Mystery Mansion", Description = "Solve puzzles and uncover secrets in a haunted house", Price = 150, Image = "", Category = null, CategoryId = 1 },
            new Game { Title = "Space Battles", Description = "Engage in epic space adventures and battles", Price = 350, Image = "", Category = null, CategoryId = 2 },
            new Game { Title = "Fantasy Realm", Description = "Explore magical lands filled with creatures and quests", Price = 220, Image = "", Category = null, CategoryId = 2 },
            new Game { Title = "Underwater Odyssey", Description = "Dive deep into the ocean and discover its mysteries", Price = 700, Image = "", Category = null, CategoryId = 4 },
            new Game { Title = "Cyberpunk City", Description = "Navigate through a futuristic city filled with danger", Price = 550, Image = "", Category = null, CategoryId = 4 },
            new Game { Title = "BBQ Party Simulator", Description = "Host the ultimate BBQ party with friends", Price = 280, Image = "", Category = null, CategoryId = 1 },
            new Game { Title = "Cooking Showdown", Description = "Compete against others in a cooking challenge", Price = 300, Image = "", Category = null, CategoryId = 2 }
            };
            context.Games.AddRange(games);
            context.SaveChanges();
        }
        [Fact]
        public async Task GetProductListAsync_ReturnsFirstPageOfThreeItems()
        {
            using var context = CreateContext();
            var service = new GameService(context);
            var result = await service.GetProductListAsync(null);
            Assert.IsType<ResponseData<ListModel<Game>>>(result);
            Assert.True(result.Successfull);
            Assert.Equal(1, result.Data.CurrentPage);
            Assert.Equal(3, result.Data.Items.Count);
            Assert.Equal(4, result.Data.TotalPages);
            Assert.Equal(context.Games.First(), result.Data.Items[0]);
        }
        [Fact]
        public async Task GetProductListAsync_ReturnsCorrectPage()
        {
            using var context = CreateContext();
            var service = new GameService(context);
            var result = await service.GetProductListAsync(null, pageNo: 4, pageSize: 3);
            Assert.True(result.Successfull);
            Assert.Equal(4, result.Data.CurrentPage);
            Assert.Single(result.Data.Items); // На 4 странице будет один объект
        }
        [Fact]
        public async Task GetProductListAsync_FiltersByCategory()
        {
            using var context = CreateContext();
            var service = new GameService(context);
            var result = await service.GetProductListAsync("strategy-games");
            Assert.True(result.Successfull);
            Assert.Equal(3, result.Data.Items.Count);
            Assert.All(result.Data.Items, item => Assert.Equal("strategy-games", item.Category.NormalizedName));
        }
        [Fact]
        public async Task GetProductListAsync_PageSizeExceedsMax_ReturnsMaxPageSize()
        {
            using var context = CreateContext();
            var service = new GameService(context);
            var result = await service.GetProductListAsync(null, pageSize: 100);
            Assert.True(result.Successfull);
            Assert.Equal(10, result.Data.Items.Count);
        }
        [Fact]
        public async Task GetProductListAsync_PageNumberExceedsTotalPages_ReturnsError()
        {
            using var context = CreateContext();
            var service = new GameService(context);
            var result = await service.GetProductListAsync(null, pageNo: 10);
            Assert.False(result.Successfull);
            Assert.Equal("Несуществующая страница", result.ErrorMessage);
        }
    }
}
