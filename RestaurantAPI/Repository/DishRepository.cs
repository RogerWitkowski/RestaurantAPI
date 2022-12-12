using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant.DataAccess.DataAccess;
using Restaurant.Models.Dto;
using Restaurant.Models.Models;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Repository.IRepository;

namespace RestaurantAPI.Repository
{
    public class DishRepository : IDishRepository
    {
        private readonly RestaurantDbContext _dbContext;
        private readonly IMapper _mapper;

        public DishRepository(RestaurantDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<int> CreateDishAsync(int restaurantId, CreateDishDto dishDto)
        {
            var restaurant = await _dbContext.Restaurants.FirstOrDefaultAsync(r => r.Id == restaurantId);
            if (restaurant is null)
            {
                throw new NotFoundException("Restaurant not found!");
            }

            var dishEntity = _mapper.Map<Dish>(dishDto);

            dishEntity.RestaurantId = restaurantId;

            await _dbContext.Dishes.AddAsync(dishEntity);
            await _dbContext.SaveChangesAsync();
            return dishEntity.Id;
        }

        public async Task<DishDto> GetDishFromRestaurantByIdAsync(int restaurantId, int dishId)
        {
            var restaurant = await _dbContext.Restaurants.FirstOrDefaultAsync(r => r.Id == restaurantId);
            if (restaurant is null)
            {
                throw new NotFoundException("Restaurant not found!");
            }

            var dish = await _dbContext.Dishes.FirstOrDefaultAsync(d => d.Id == dishId);
            if (dish is null || dish.RestaurantId != restaurantId)
            {
                throw new NotFoundException("Dish not found!");
            }

            var dishDto = _mapper.Map<DishDto>(dish);
            return dishDto;
        }
    }
}