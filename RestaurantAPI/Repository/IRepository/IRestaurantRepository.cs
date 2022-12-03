using Microsoft.AspNetCore.Mvc;

namespace RestaurantAPI.Repository.IRepository
{
    public interface IRestaurantRepository
    {
        public Task<ActionResult<IEnumerable<Restaurant.Models.Models.Restaurant>>> GetAll();

        public Task<ActionResult<Restaurant.Models.Models.Restaurant>> GetById(int id);
    }
}