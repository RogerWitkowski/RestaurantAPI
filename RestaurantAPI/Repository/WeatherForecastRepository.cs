using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Repository.IRepository;

namespace RestaurantAPI.Repository
{
    public class WeatherForecastRepository : IWeatherForecastRepository
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [HttpGet]
        public IEnumerable<WeatherForecast> Get(int count, int minTemperature, int maxTemperature)
        {
            return Enumerable.Range(1, count).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(minTemperature, maxTemperature),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
                .ToArray();
        }
    }
}