using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Repository.IRepository;

namespace RestaurantAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IWeatherForecastRepository _forecast;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherForecastRepository forecast)
        {
            _logger = logger;
            _forecast = forecast;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var result = _forecast.Get(15, -35, 50);
            return result;
        }

        [HttpPost("generate")]
        public ActionResult<IEnumerable<WeatherForecast>> Generate([FromQuery] int count,
            [FromBody] TemperatureRequest request)
        {
            if (count < 0 || request.MaxTemperature < request.MinTemperature)
            {
                return BadRequest();
            }

            var result = _forecast.Get(count, request.MinTemperature, request.MinTemperature);
            return Ok(result);
        }
    }
}