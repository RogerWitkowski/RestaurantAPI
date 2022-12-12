using Microsoft.AspNetCore.Mvc;
using Restaurant.Models.Dto;

namespace RestaurantAPI.Repository.IRepository
{
    public interface IDishRepository
    {
        public Task<int> CreateDishAsync(int restaurantId, CreateDishDto dishDto);

        public Task<DishDto> GetDishFromRestaurantByIdAsync(int restaurantId, int dishId);
    }
}