using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Restaurant.DataAccess.DataAccess;
using Restaurant.Models.Dto;
using Restaurant.Models.Models;
using Restaurant.Utility;
using RestaurantAPI.Authorization;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Repository.IRepository;

namespace RestaurantAPI.Repository
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly RestaurantDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RestaurantRepository> _logger;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserContextRepository _userContextRepository;
        private readonly IExtensionRepository _extensionRepository;

        public RestaurantRepository(RestaurantDbContext dbContext, IMapper mapper, ILogger<RestaurantRepository> logger, IAuthorizationService authorizationService, IUserContextRepository userContextRepository)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _authorizationService = authorizationService;
            _userContextRepository = userContextRepository;
            _extensionRepository = new ExtensionRepository(dbContext);
        }

        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateRestaurantAsync(CreateRestaurantDto createRestaurantDto)
        {
            var restaurant = _mapper.Map<Restaurant.Models.Models.Restaurant>(createRestaurantDto);

            restaurant.CreatedById = _userContextRepository.GetUserId;
            await _dbContext.Restaurants.AddAsync(restaurant);
            await _dbContext.SaveChangesAsync();

            return new CreatedResult($"/api/restaurant/{restaurant.Id}", "Created successfully!");
        }

        [ValidateAntiForgeryToken]
        public async Task DeleteRestaurantAsync(int restaurantId)
        {
            _logger.LogError($"Restaurant with id: {restaurantId} DELETE action invoked");
            var restaurant = await _extensionRepository.GetRestaurantByIdAsync(restaurantId);

            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextRepository.User, restaurant,
                new ResourceOperationRequirement(ResourceOperationStaticDetails.Delete)).Result;
            if (!authorizationResult.Succeeded)
            {
                throw new NotFoundException("Not Found!");
            }
            _dbContext.Restaurants.Remove(restaurant);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<PagedResult<RestaurantDto>> GetAllRestaurantsPaged(RestaurantQuery restaurantQuery)
        {
            var baseRestaurantQuery = await _dbContext
                .Restaurants
                .Include(a => a.Address)
                //.Include(d => d.Dishes)
                .Where(r => restaurantQuery.SearchPhrase == null ||
                            (r.Name.ToLower().Contains(restaurantQuery.SearchPhrase.ToLower())
                             || r.Description.ToLower().Contains(restaurantQuery.SearchPhrase.ToLower()))).ToListAsync();

            var restaurants = Task.FromResult(baseRestaurantQuery
                .Skip(restaurantQuery.PageSize * (restaurantQuery.PageNumber - 1))
                .Take(restaurantQuery.PageSize));

            var totalItemsCount = baseRestaurantQuery.Count();
            if (restaurants.Result is null)
            {
                throw new NotFoundException("Restaurant not found!");
            }

            var restaurantsDto = _mapper.Map<List<RestaurantDto>>(restaurants.Result);

            var pagedResult =
                new PagedResult<RestaurantDto>(restaurantsDto, totalItemsCount, restaurantQuery.PageSize, restaurantQuery.PageNumber);

            if (!pagedResult.Items.Any())
            {
                throw new NotFoundException("Not Found");
            }

            return pagedResult;
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

            var authorizationResult = await _authorizationService.AuthorizeAsync(_userContextRepository.User, restaurant,
                new ResourceOperationRequirement(ResourceOperationStaticDetails.Update));

            if (!authorizationResult.Succeeded)
            {
                throw new NotFoundException("Not Found!");
            }

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