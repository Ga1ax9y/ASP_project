using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Infrastructure;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using WEB_253505_Stanishewski.Domain.Entities;
using WEB_253505_Stanishewski.Domain.Models;
using WEB_253505_Stanishewski.UI.Controllers;
using WEB_253505_Stanishewski.UI.Services.GameService;
using WEB_253505_Stanishewski.UI.Services.CategoryService;
using WEB_253505_Stanishewski.Tests.Serializer;

namespace WEB_253505_Stanishewski.Tests
{
    public class ProductControllerTests
    {
        private readonly IGameService _gameService;
        private readonly ICategoryService _categoryService;
        private readonly ProductController _productController;
        private List<Game> games = new List<Game>
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
            new Game { Title = "Cooking Showdown", Description = "Compete against others in a cooking challenge", Price = 300, Image = "", Category = null, CategoryId = 1 }
        };
        private List<Category> categories = new List<Category>
        {
            new Category { Id = 1, Name = "Strategy Games", NormalizedName = "strategy-games" },
            new Category { Id = 2, Name = "Adventure Games", NormalizedName = "adventure-games" },
            new Category { Id = 3, Name = "Action Games", NormalizedName = "action-games" },
            new Category { Id = 4, Name = "Simulation Games", NormalizedName = "simulation-games" },
            new Category { Id = 5, Name = "Puzzle Games", NormalizedName = "puzzle-games" },
            new Category { Id = 6, Name = "Party Games", NormalizedName = "party-games" }
        };
        public ProductControllerTests()
        {
            _gameService = Substitute.For<IGameService>();
            _categoryService = Substitute.For<ICategoryService>();
            var controllerContext = new ControllerContext();
            var fakeHttpContext = Substitute.For<HttpContext>();
            var fakeHttpRequest = Substitute.For<HttpRequest>();
            var fakeServices = new ServiceCollection();
            fakeServices.AddSingleton<TempDataSerializer, TempSerializer>();
            fakeServices.AddSingleton<ITempDataProvider, SessionStateTempDataProvider>();
            fakeServices.AddSingleton<ITempDataDictionaryFactory, TempDataDictionaryFactory>();
            var fakeServiceProvider = fakeServices.BuildServiceProvider();
            fakeHttpContext.RequestServices = fakeServiceProvider;
            var headerDictionary = new HeaderDictionary(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues> { /* { "x-requested-with", "XMLHttpRequest" } */ });
            
            fakeHttpRequest.Headers.Returns(headerDictionary);
            
            fakeHttpContext.Request.Returns(fakeHttpRequest);
            controllerContext.HttpContext = fakeHttpContext;
            _productController = new ProductController(_gameService, _categoryService) { ControllerContext = controllerContext };
        }
        [Fact]
        public async Task Index_ReturnsViewResults_WithCategoryService()
        {
            _categoryService.GetCategoryListAsync().Returns(Task.FromResult(new ResponseData<List<Category>>()
            {
                Data = categories
            }));
            _gameService.GetProductListAsync(null).Returns(Task.FromResult(new ResponseData<ListModel<Game>>()
            {
                Data = new ListModel<Game>
                {
                    Items = games,
                    CurrentPage = 1,
                    TotalPages = 5,
                }
            }));
            var result = await _productController.Index(null, 1) as ViewResult;
            Assert.NotNull(result);
            Assert.NotEqual(404, result.StatusCode);
            Assert.Equal("Все", result.ViewData["currentCategory"]);
            var viewDataCategories = result.ViewData["categories"] as List<Category>;
            Assert.NotNull(viewDataCategories);
            Assert.Equal(categories.Count, viewDataCategories.Count);
            Assert.Equal(categories[0].Name, viewDataCategories[0].Name);
            var model = result.Model as ListModel<Game>;
            Assert.NotNull(model);
            Assert.Equal(games.Count, model.Items.Count);
            Assert.Equal(games[0].Title, model.Items[0].Title);
        }
        [Fact]
        public async Task Index_ReturnsNotFound_WhenCategoriesFetchFails()
        {

            _categoryService.GetCategoryListAsync().Returns(Task.FromResult(new ResponseData<List<Category>>()
            {
                Successfull = false,
                ErrorMessage = "Failed to fetch categories."
            }));

            var result = await _productController.Index(null, 1) as NotFoundObjectResult;

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, result.StatusCode);
            Assert.Equal("Failed to fetch categories.", notFoundResult.Value);
        }
        [Fact]
        public async Task Index_ReturnsNotFound_WhenGamesFetchFails()
        {

            _categoryService.GetCategoryListAsync().Returns(Task.FromResult(new ResponseData<List<Category>>()
            {
                Successfull = true,
                Data = categories
            }));
            _gameService.GetProductListAsync(null).Returns(Task.FromResult(new ResponseData<ListModel<Game>>()
            {
                Successfull = false,
                ErrorMessage = "Failed to fetch games."
            }));

            var result = await _productController.Index(null, 1) as NotFoundObjectResult;

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, result.StatusCode);
            Assert.Equal("Failed to fetch games.", notFoundResult.Value);
        }
    }
}