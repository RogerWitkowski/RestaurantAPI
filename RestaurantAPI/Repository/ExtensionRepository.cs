using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant.DataAccess.DataAccess;
using Restaurant.Models.Dto;
using Restaurant.Models.Models;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Repository.IRepository;

namespace RestaurantAPI.Repository
{
    public class ExtensionRepository : IExtensionRepository
    {
        private readonly RestaurantDbContext _dbContext;

        public ExtensionRepository(RestaurantDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ActionResult<Restaurant.Models.Models.Restaurant>> GetRestaurantWithDishesByIdAsync(int restaurantId)
        {
            var restaurant = await _dbContext
                .Restaurants
                .Include(d => d.Dishes)
                .FirstOrDefaultAsync(r => r.Id == restaurantId);

            if (restaurant is null)
            {
                throw new NotFoundException("Restaurant not found!");
            }

            return restaurant;
        }

        public async Task<Restaurant.Models.Models.Restaurant> GetRestaurantByIdAsync(int restaurantId)
        {
            var restaurant = await _dbContext
                .Restaurants
                .FirstOrDefaultAsync(r => r.Id == restaurantId);

            if (restaurant is null)
            {
                throw new NotFoundException("Restaurant not found!");
            }
            return restaurant;
        }

        public async Task<Dish> GetDishByIdFromRestaurantAsync(int restaurantId, int dishId)
        {
            var dish = await _dbContext.Dishes.FirstOrDefaultAsync(d => d.Id == dishId);
            if (dish is null || dish.RestaurantId != restaurantId)
            {
                throw new NotFoundException("Dish not found!");
            }

            return dish;
        }

        public async Task<IEnumerable<Restaurant.Models.Models.Restaurant>> GetAllRestaurantsAsync()
        {
            var restaurants = await _dbContext
                .Restaurants
                .Include(a => a.Address)
                .Include(d => d.Dishes)
                .ToListAsync();

            if (restaurants is null)
            {
                throw new NotFoundException("Restaurant not found!");
            }

            return restaurants;
        }

        public async Task<Restaurant.Models.Models.Restaurant> GetRestaurantWithAddressAndDishesByIdAsync(int restaurantId)
        {
            var restaurant = await _dbContext
                .Restaurants
                .Include(a => a.Address)
                .Include(d => d.Dishes)
                .FirstOrDefaultAsync(i => i.Id == restaurantId);

            if (restaurant is null)
            {
                throw new NotFoundException("Restaurant not found!");
            }

            return restaurant;
        }

        public async Task<Restaurant.Models.Models.Restaurant> GetRestaurantWithAddressByIdAsync(int restaurantId)
        {
            var restaurant = await _dbContext
                .Restaurants
                .Include(a => a.Address)
                .FirstOrDefaultAsync(r => r.Id == restaurantId);

            if (restaurant is null)
            {
                throw new NotFoundException("Restaurant not found!");
            }

            return restaurant;
        }

        public async Task<User> GetUserWithRoleFromDbByEmailLoginAsync(LoginDto loginDto)
        {
            var user = await _dbContext
                .Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email);
            if (user is null)
            {
                throw new BadRequestException("Invalid username or password!");
            }

            return user;
        }
    }
}