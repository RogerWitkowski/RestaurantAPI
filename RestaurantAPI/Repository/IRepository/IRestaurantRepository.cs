using Microsoft.AspNetCore.Mvc;
using Restaurant.Models.Dto;

namespace RestaurantAPI.Repository.IRepository
{
    public interface IRestaurantRepository
    {
        public Task<ActionResult<IEnumerable<RestaurantDto>>> GetAllAsync();

        public Task<ActionResult<RestaurantDto>> GetByIdAsync(int id);

        public Task<ActionResult> CreateRestaurantAsync(CreateRestaurantDto createRestaurantDto);

        public Task<bool> DeleteRestaurantAsync(int restaurantId);

        public Task<bool> UpdateRestaurantAsync(int restaurantId, UpdateRestaurantDto restaurantDto);
    }
}