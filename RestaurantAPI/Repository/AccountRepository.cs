using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Restaurant.DataAccess.DataAccess;
using Restaurant.Models.Dto;
using Restaurant.Models.Models;
using RestaurantAPI.Repository.IRepository;

namespace RestaurantAPI.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly RestaurantDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AccountRepository(RestaurantDbContext dbContext, IMapper mapper, IPasswordHasher<User> passwordHasher)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
        }

        public async Task<ActionResult> RegisterUserAsync(RegisterUserDto registerUserDto)
        {
            var newUser = _mapper.Map<User>(registerUserDto);

            var hashedPassword = _passwordHasher.HashPassword(newUser, registerUserDto.Password);

            newUser.PasswordHash = hashedPassword;
            await _dbContext.Users.AddAsync(newUser);
            await _dbContext.SaveChangesAsync();

            return new CreatedResult($"api/account/register/{newUser.FirsttName}", "Created!");
        }
    }
}