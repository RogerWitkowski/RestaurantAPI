using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;

namespace RestaurantAPI.Repository.IRepository
{
    public interface IWeatherForecastRepository
    {
        public IEnumerable<WeatherForecast> Get(int count, int minTemperature, int maxTemperature);
    }
}