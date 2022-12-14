using Microsoft.AspNetCore.Mvc;
using Restaurant.Models.Dto;
using Restaurant.Models.Models;

namespace RestaurantAPI.Repository.IRepository
{
    public interface IAccountRepository
    {
        public Task<ActionResult> RegisterUserAsync(RegisterUserDto registerUserDto);

        public Task<string> GenerateJwTAsync(LoginDto loginDto);
    }
}