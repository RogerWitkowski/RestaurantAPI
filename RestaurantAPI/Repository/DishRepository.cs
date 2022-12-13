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
        private readonly IExtensionRepository _extensionRepository;

        public DishRepository(RestaurantDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _extensionRepository = new ExtensionRepository(dbContext);
        }

        [ValidateAntiForgeryToken]
        public async Task<int> CreateDishAsync(int restaurantId, CreateDishDto dishDto)
        {
            await _extensionRepository.GetRestaurantByIdAsync(restaurantId);

            var dishEntity = _mapper.Map<Dish>(dishDto);

            dishEntity.RestaurantId = restaurantId;

            await _dbContext.Dishes.AddAsync(dishEntity);
            await _dbContext.SaveChangesAsync();
            return dishEntity.Id;
        }

        public async Task<ActionResult<List<DishDto>>> GetAllDishFromRestaurantAsync(int restaurantId)
        {
            var restaurant = await _extensionRepository.GetRestaurantWithDishesByIdAsync(restaurantId);

            var dishesDto = _mapper.Map<List<DishDto>>(restaurant.Value.Dishes);
            return dishesDto;
        }

        public async Task RemoveAllDishesFromRestaurantAsync(int restaurantId)
        {
            var restaurant = await _extensionRepository.GetRestaurantWithDishesByIdAsync(restaurantId);
            _dbContext.RemoveRange(restaurant.Value.Dishes);
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveDishFromRestaurantByDishIdAsync(int restaurantId, int dishId)
        {
            await _extensionRepository.GetRestaurantByIdAsync(restaurantId);

            var dish = await _extensionRepository.GetDishByIdFromRestaurantAsync(restaurantId, dishId);

            _dbContext.RemoveRange(dish);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<DishDto> GetDishFromRestaurantByIdAsync(int restaurantId, int dishId)
        {
            await _extensionRepository.GetRestaurantByIdAsync(restaurantId);

            var dish = await _extensionRepository.GetDishByIdFromRestaurantAsync(restaurantId, dishId);

            var dishDto = _mapper.Map<DishDto>(dish);
            return dishDto;
        }
    }
}