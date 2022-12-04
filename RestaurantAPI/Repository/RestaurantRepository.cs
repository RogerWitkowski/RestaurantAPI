﻿using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Restaurant.DataAccess.DataAccess;
using Restaurant.Models.Dto;
using RestaurantAPI.Repository.IRepository;

namespace RestaurantAPI.Repository
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly RestaurantDbContext _dbContext;
        private readonly IMapper _mapper;

        public RestaurantRepository(RestaurantDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ActionResult> CreateRestaurantAsync(CreateRestaurantDto createRestaurantDto)
        {
            var restaurant = _mapper.Map<Restaurant.Models.Models.Restaurant>(createRestaurantDto);

            await _dbContext.Restaurants.AddAsync(restaurant);
            await _dbContext.SaveChangesAsync();

            return new CreatedResult($"/api/restaurant/{restaurant.Id}", "Created successfully!");
        }

        public async Task<bool> DeleteRestaurantAsync(int restaurantId)
        {
            var restaurant = await _dbContext.Restaurants.FirstOrDefaultAsync(r => r.Id == restaurantId);
            if (restaurant != null)
            {
                _dbContext.Restaurants.Remove(restaurant);
                await _dbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetAllAsync()
        {
            var restaurants = await _dbContext
                .Restaurants
                .Include(a => a.Address)
                .Include(d => d.Dishes)
                .ToListAsync();

            var restaurantsDto = _mapper.Map<List<RestaurantDto>>(restaurants);

            return restaurantsDto;
        }

        public async Task<ActionResult<RestaurantDto>> GetByIdAsync(int id)
        {
            var restaurant = await _dbContext
                .Restaurants
                .Include(a => a.Address)
                .Include(d => d.Dishes)
                .FirstOrDefaultAsync(i => i.Id == id);

            //if (restaurant is null)
            //{
            //    return new NotFoundObjectResult("Something went wrong. 404 Not Found");
            //}

            var restaurantDto = _mapper.Map<RestaurantDto>(restaurant);
            return restaurantDto;
        }
    }
}