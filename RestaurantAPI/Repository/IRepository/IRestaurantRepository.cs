using Microsoft.AspNetCore.Mvc;
using Restaurant.Models.Dto;
using System.Security.Claims;

namespace RestaurantAPI.Repository.IRepository
{
    public interface IRestaurantRepository
    {
        public Task<ActionResult<IEnumerable<RestaurantDto>>> GetAllAsync();

        public Task<ActionResult<RestaurantDto>> GetByIdAsync(int restaurantId);

        public Task<ActionResult> CreateRestaurantAsync(CreateRestaurantDto createRestaurantDto, int userId);

        public Task DeleteRestaurantAsync(int restaurantId, ClaimsPrincipal user);

        public Task UpdateRestaurantAsync(int restaurantId, UpdateRestaurantDto restaurantDto, ClaimsPrincipal user);
    }
}