using Microsoft.EntityFrameworkCore;
using WEB_253505_Stanishewski.Domain.Entities;

namespace WEB_253505_Stanishewski.UI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() { }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Default");
        }
        public DbSet<Game> Games { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
