using Microsoft.AspNetCore.Mvc;
using Restaurant.Models.Dto;
using Restaurant.Models.Models;
using System.Security.Claims;

namespace RestaurantAPI.Repository.IRepository
{
    public interface IRestaurantRepository
    {
        public Task<PagedResult<RestaurantDto>> GetAllRestaurantsPaged(RestaurantQuery restaurantQuery);

        public Task<ActionResult<RestaurantDto>> GetByIdAsync(int restaurantId);

        public Task<ActionResult> CreateRestaurantAsync(CreateRestaurantDto createRestaurantDto);

        public Task DeleteRestaurantAsync(int restaurantId);

        public Task UpdateRestaurantAsync(int restaurantId, UpdateRestaurantDto restaurantDto);
    }
}