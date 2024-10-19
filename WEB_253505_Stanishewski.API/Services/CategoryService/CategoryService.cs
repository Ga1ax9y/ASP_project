using Microsoft.EntityFrameworkCore;
using WEB_253505_Stanishewski.API.Data;
using WEB_253505_Stanishewski.API.Services.GameService;
using WEB_253505_Stanishewski.Domain.Entities;
using WEB_253505_Stanishewski.Domain.Models;

namespace WEB_253505_Stanishewski.API.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;
        public CategoryService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ResponseData<List<Category>>> GetCategoryListAsync()
        {
            var categories = await _context.Categories.ToListAsync();
            return ResponseData<List<Category>>.Success(categories);
        }
    }
}
