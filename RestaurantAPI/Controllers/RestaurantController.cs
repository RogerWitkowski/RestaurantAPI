using System.Reflection;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult<IEnumerable<Restaurant.Models.Models.Restaurant>>> GetAll()
        {
            var restaurants = await _restaurantRepository.GetAll();
            return Ok(restaurants);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Restaurant.Models.Models.Restaurant>> GetById([FromRoute] int id)
        {
            var restaurant = await _restaurantRepository.GetById(id);
            return restaurant;
        }
    }
}