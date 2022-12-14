using Microsoft.AspNetCore.Mvc;
using Restaurant.Models.Dto;
using Restaurant.Models.Models;

namespace RestaurantAPI.Repository.IRepository
{
    public interface IExtensionRepository
    {
        public Task<ActionResult<Restaurant.Models.Models.Restaurant>> GetRestaurantWithDishesByIdAsync(int restaurantId);

        public Task<Restaurant.Models.Models.Restaurant> GetRestaurantByIdAsync(int restaurantId);

        public Task<Dish> GetDishByIdFromRestaurantAsync(int restaurantId, int dishId);

        public Task<IEnumerable<Restaurant.Models.Models.Restaurant>> GetAllRestaurantsAsync();

        public Task<Restaurant.Models.Models.Restaurant> GetRestaurantWithAddressAndDishesByIdAsync(int restaurantId);

        public Task<Restaurant.Models.Models.Restaurant> GetRestaurantWithAddressByIdAsync(int restaurantId);

        public Task<User> GetUserWithRoleFromDbByEmailLoginAsync(LoginDto loginDto);
    }
}