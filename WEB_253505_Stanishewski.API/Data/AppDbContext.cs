using Microsoft.EntityFrameworkCore;
using WEB_253505_Stanishewski.Domain.Entities;

namespace WEB_253505_Stanishewski.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Game> Games { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
