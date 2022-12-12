using Microsoft.AspNetCore.Mvc;
using Restaurant.Models.Dto;
using RestaurantAPI.Repository.IRepository;

namespace RestaurantAPI.Controllers
{
    [Route("api/restaurant/{restaurantId}/dish")]
    [ApiController]
    public class DishController : ControllerBase
    {
        private readonly IDishRepository _dishRepository;

        public DishController(IDishRepository dishRepository)
        {
            _dishRepository = dishRepository;
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateDish([FromRoute] int restaurantId, [FromBody] CreateDishDto dishDto)
        {
            var newDishId = await _dishRepository.CreateDishAsync(restaurantId, dishDto);

            return Created($"api/restaurant/{restaurantId}/dish/{newDishId}", null);
        }

        [HttpGet("{dishId:int}")]
        public async Task<ActionResult<DishDto>> GetDishFromRestaurantById([FromRoute] int restaurantId, [FromRoute] int dishId)
        {
            var dish = await _dishRepository.GetDishFromRestaurantByIdAsync(restaurantId, dishId);
            return Ok(dish);
        }
    }
}