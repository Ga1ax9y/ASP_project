using Microsoft.EntityFrameworkCore;
using WEB_253505_Stanishewski.API.Data;
using WEB_253505_Stanishewski.Domain.Entities;
using WEB_253505_Stanishewski.Domain.Models;

namespace WEB_253505_Stanishewski.API.Services.GameService
{
    public class GameService : IGameService
    {
        private readonly AppDbContext _context;
        private readonly int _maxPageSize = 20;
        public GameService(AppDbContext context)
        {
            _context = context;
        }
        public Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile)
        {
            throw new NotImplementedException();
        }
        public async Task<ResponseData<Game>> CreateProductAsync(Game product)
        {
            await _context.Games.AddAsync(product);
            await _context.SaveChangesAsync();

            return ResponseData<Game>.Success(product);
        }
        public async Task DeleteProductAsync(int id)
        {
            var dish = await _context.Games.FindAsync(id);
            if (dish == null)
            {
                throw new Exception("Блюдо не найдено");
            }
            _context.Games.Remove(dish);
            await _context.SaveChangesAsync();
        }
        public async Task<ResponseData<Game>> GetProductByIdAsync(int id)
        {
            var dish = await _context.Games.Include(d => d.Category).FirstOrDefaultAsync(d => d.Id == id);
            if (dish == null)
            {
                return ResponseData<Game>.Error("Товар не найден");
            }
            return ResponseData<Game>.Success(dish);
        }
        public async Task<ResponseData<ListModel<Game>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1, int pageSize = 3)
        {
            if (pageSize > _maxPageSize)
                pageSize = _maxPageSize;
            var query = _context.Games.AsQueryable();
            var dataList = new ListModel<Game>();
            query = query.Where(d => categoryNormalizedName == null || d.Category.NormalizedName.Equals(categoryNormalizedName));
            var count = await query.CountAsync();
            if (count == 0)
            {
                return ResponseData<ListModel<Game>>.Success(dataList);
            }
            int totalPages = (int)Math.Ceiling(count / (double)pageSize);
            if (pageNo > totalPages)
                return ResponseData<ListModel<Game>>.Error("Нет такой страницы");
            dataList.Items = await query
                .OrderBy(d => d.Id)
                .Skip((pageNo - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            dataList.CurrentPage = pageNo;
            dataList.TotalPages = totalPages;

            return ResponseData<ListModel<Game>>.Success(dataList);
        }
        public async Task UpdateProductAsync(int id, Game product)
        {
            var existingDish = await _context.Games.FindAsync(id);
            if (existingDish == null)
            {
                throw new Exception("Блюдо не найдено");
            }
            existingDish.Title = product.Title;
            existingDish.Description = product.Description;
            existingDish.Price = product.Price;
            existingDish.CategoryId = product.CategoryId;
            existingDish.Image = product.Image;
            await _context.SaveChangesAsync();
        }
    }
}
