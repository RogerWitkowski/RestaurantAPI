using System.Net;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant.DataAccess.DataAccess;
using RestaurantAPI.Repository.IRepository;

namespace RestaurantAPI.Repository
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly RestaurantDbContext _dbContext;

        public RestaurantRepository(RestaurantDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ActionResult<IEnumerable<Restaurant.Models.Models.Restaurant>>> GetAll()
        {
            var restaurants = await _dbContext
                .Restaurants
                .Include(a => a.Address)
                .Include(d => d.Dishes)
                .ToListAsync();
            return restaurants;
        }

        public async Task<ActionResult<Restaurant.Models.Models.Restaurant>> GetById(int id)
        {
            var restaurant = await _dbContext
                .Restaurants
                .Include(a => a.Address)
                .Include(d => d.Dishes)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (restaurant is null)
            {
                return new NotFoundObjectResult("Something went wrong. 404 Not Found");
            }
            return restaurant;
        }
    }
}