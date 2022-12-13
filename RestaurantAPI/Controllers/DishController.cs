using Microsoft.AspNetCore.Mvc;
using Restaurant.Models.Dto;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Repository.IRepository;

namespace RestaurantAPI.Controllers
{
    [Route("api/restaurant/{restaurantId:int}/dish")]
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

            return Created($"api/restaurant/{restaurantId}/dish/{newDishId}", "Created!");
        }

        [HttpGet("{dishId:int}")]
        public async Task<ActionResult<DishDto>> GetDishFromRestaurantById([FromRoute] int restaurantId, [FromRoute] int dishId)
        {
            var dish = await _dishRepository.GetDishFromRestaurantByIdAsync(restaurantId, dishId);
            return Ok(dish);
        }

        [HttpGet]
        public async Task<ActionResult<List<DishDto>>> GetAllDishes([FromRoute] int restaurantId)
        {
            var dishes = await _dishRepository.GetAllDishFromRestaurantAsync(restaurantId);
            if (!dishes.Value.Any())
            {
                throw new NotFoundException("Dishes not found yet!");
            }
            return Ok(dishes.Value);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteAllDishesFromRestaurant([FromRoute] int restaurantId)
        {
            await _dishRepository.RemoveAllDishesFromRestaurantAsync(restaurantId);
            return NoContent();
        }

        [HttpDelete("{dishId:int}")]
        public async Task<ActionResult> DeleteDishFromRestaurantByDishId([FromRoute] int restaurantId,
            [FromRoute] int dishId)
        {
            await _dishRepository.RemoveDishFromRestaurantByDishIdAsync(restaurantId, dishId);
            return NoContent();
        }
    }
}