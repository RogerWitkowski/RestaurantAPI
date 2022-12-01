using Microsoft.EntityFrameworkCore;
using Restaurant.Models.Models;

namespace Restaurant.DataAccess.DataAccess
{
    public class RestaurantDbContext : DbContext
    {
        public DbSet<Models.Models.Restaurant> Restaurants { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Dish> Dishes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dish>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");
        }
    }
}