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
            return Ok(restaurants);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RestaurantDto>> GetById([FromRoute] int id)
        {
            var restaurant = await _restaurantRepository.GetByIdAsync(id);

            if (restaurant.Result != null || restaurant.Value != null)
            {
                return Ok(restaurant);
            }
            return NotFound("Something went wrong. 404 Not Found!");
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateRestaurant([FromBody] CreateRestaurantDto createRestaurantDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var restaurant = await _restaurantRepository.CreateRestaurantAsync(createRestaurantDto);
            return restaurant;
        }

        [HttpDelete("{restaurantId:int}")]
        public async Task<ActionResult> DeleteRestaurant([FromRoute] int restaurantId)
        {
            var isDeleted = await _restaurantRepository.DeleteRestaurantAsync(restaurantId);
            if (isDeleted)
            {
                return Ok("Deleted!");
            }

            return NotFound("Something went wrong. 404 Not Found!");
        }

        [HttpPut("{restaurantId:int}")]
        public async Task<ActionResult> UpdateRestaurant([FromBody] UpdateRestaurantDto restaurantDto, [FromRoute] int restaurantId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var isUpdatedRestaurant = await _restaurantRepository.UpdateRestaurantAsync(restaurantId, restaurantDto);
            if (!isUpdatedRestaurant)
            {
                return NotFound("Something went wrong. 404 Not Found!");
            }

            return Ok("Updated!");
        }
    }
}