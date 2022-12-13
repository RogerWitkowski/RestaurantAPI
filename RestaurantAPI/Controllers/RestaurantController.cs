using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Restaurant.Models.Dto;
using RestaurantAPI.Repository.IRepository;

namespace RestaurantAPI.Controllers
{
    [Route("api/restaurant")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantRepository _restaurantRepository;

        public RestaurantController(IRestaurantRepository restaurantRepository)
        {
            _restaurantRepository = restaurantRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetAll()
        {
            var restaurants = await _restaurantRepository.GetAllAsync();
            return Ok(restaurants.Value);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RestaurantDto>> GetById([FromRoute] int id)
        {
            var restaurant = await _restaurantRepository.GetByIdAsync(id);

            return Ok(restaurant.Value);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateRestaurant([FromBody] CreateRestaurantDto createRestaurantDto)
        {
            var restaurant = await _restaurantRepository.CreateRestaurantAsync(createRestaurantDto);
            return restaurant;
        }

        [HttpDelete("{restaurantId:int}")]
        public async Task<ActionResult> DeleteRestaurant([FromRoute] int restaurantId)
        {
            await _restaurantRepository.DeleteRestaurantAsync(restaurantId);
            return Ok("Deleted!");
        }

        [HttpPut("{restaurantId:int}")]
        public async Task<ActionResult> UpdateRestaurant([FromBody] UpdateRestaurantDto restaurantDto, [FromRoute] int restaurantId)
        {
            await _restaurantRepository.UpdateRestaurantAsync(restaurantId, restaurantDto);

            return Ok("Updated!");
        }
    }
}