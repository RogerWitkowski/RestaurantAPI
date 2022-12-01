using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Restaurant.Models.Models;

namespace Restaurant.DataAccess.DataAccess
{
    public class RestaurantDbContext : DbContext
    {
        public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options) : base(options)
        {
        }

        public DbSet<Models.Models.Restaurant> Restaurants { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Dish> Dishes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.Models.Restaurant>(entityBuilder =>
            {
                entityBuilder.Property(n => n.Name).IsRequired().HasMaxLength(50);
                entityBuilder.Property(d => d.Description).IsRequired().HasMaxLength(100);
            });

            modelBuilder.Entity<Dish>(entityBuilder =>
            {
                entityBuilder.Property(n => n.Name).IsRequired();
                entityBuilder.Property(p => p.Price).HasColumnType("decimal(18,2)");
            });
        }
    }
}