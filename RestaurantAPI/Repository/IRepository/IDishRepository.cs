using Microsoft.AspNetCore.Mvc;
using Restaurant.Models.Dto;

namespace RestaurantAPI.Repository.IRepository
{
    public interface IDishRepository
    {
        public Task<int> CreateDishAsync(int restaurantId, CreateDishDto dishDto);

        public Task<DishDto> GetDishFromRestaurantByIdAsync(int restaurantId, int dishId);

        public Task<ActionResult<List<DishDto>>> GetAllDishFromRestaurantAsync(int restaurantId);

        public Task RemoveAllDishesFromRestaurantAsync(int restaurantId);

        public Task RemoveDishFromRestaurantByDishIdAsync(int restaurantId, int dishId);
    }
}