using Microsoft.AspNetCore.Mvc;
using Restaurant.Models.Dto;

namespace RestaurantAPI.Repository.IRepository
{
    public interface IRestaurantRepository
    {
        public Task<ActionResult<IEnumerable<RestaurantDto>>> GetAll();

        public Task<ActionResult<RestaurantDto>> GetById(int id);
    }
}