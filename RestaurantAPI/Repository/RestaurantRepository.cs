using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Restaurant.DataAccess.DataAccess;
using Restaurant.Models.Dto;
using Restaurant.Models.Models;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Repository.IRepository;

namespace RestaurantAPI.Repository
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly RestaurantDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RestaurantRepository> _logger;
        private readonly IExtensionRepository _extensionRepository;

        public RestaurantRepository(RestaurantDbContext dbContext, IMapper mapper, ILogger<RestaurantRepository> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _extensionRepository = new ExtensionRepository(dbContext);
        }

        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateRestaurantAsync(CreateRestaurantDto createRestaurantDto)
        {
            var restaurant = _mapper.Map<Restaurant.Models.Models.Restaurant>(createRestaurantDto);

            await _dbContext.Restaurants.AddAsync(restaurant);
            await _dbContext.SaveChangesAsync();

            return new CreatedResult($"/api/restaurant/{restaurant.Id}", "Created successfully!");
        }

        [ValidateAntiForgeryToken]
        public async Task DeleteRestaurantAsync(int restaurantId)
        {
            _logger.LogError($"Restaurant with id: {restaurantId} DELETE action invoked");
            var restaurant = await _extensionRepository.GetRestaurantByIdAsync(restaurantId);

            _dbContext.Restaurants.Remove(restaurant);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetAllAsync()
        {
            var restaurants = await _extensionRepository.GetAllRestaurantsAsync();

            var restaurantsDto = _mapper.Map<List<RestaurantDto>>(restaurants);

            return restaurantsDto;
        }

        [ValidateAntiForgeryToken]
        public async Task<ActionResult<RestaurantDto>> GetByIdAsync(int restaurantId)
        {
            var restaurant = await _extensionRepository.GetRestaurantWithAddressAndDishesByIdAsync(restaurantId);
            var restaurantDto = _mapper.Map<RestaurantDto>(restaurant);
            return restaurantDto;
        }

        [ValidateAntiForgeryToken]
        public async Task UpdateRestaurantAsync(int restaurantId, UpdateRestaurantDto restaurantDto)
        {
            var restaurant = await _extensionRepository.GetRestaurantWithAddressByIdAsync(restaurantId);

            restaurant.Name = restaurantDto.Name;
            restaurant.Description = restaurantDto.Description;
            restaurant.Category = restaurantDto.Category;
            restaurant.HasDelivery = restaurantDto.HasDelivery;
            restaurant.ContactEmail = restaurantDto.ContactEmail;
            restaurant.ContactNumber = restaurantDto.ContactNumber;
            restaurant.Address.Country = restaurantDto.Country;
            restaurant.Address.City = restaurantDto.City;
            restaurant.Address.Street = restaurantDto.Street;
            restaurant.Address.PostalCode = restaurantDto.PostalCode;

            await _dbContext.SaveChangesAsync();
        }
    }
}