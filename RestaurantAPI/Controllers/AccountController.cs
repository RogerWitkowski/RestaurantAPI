using Microsoft.AspNetCore.Mvc;
using Restaurant.Models.Dto;
using Restaurant.Models.Models;
using RestaurantAPI.Repository.IRepository;

namespace RestaurantAPI.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser([FromBody] RegisterUserDto registerUserDto)
        {
            var newUser = await _accountRepository.RegisterUserAsync(registerUserDto);
            return newUser;
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> UserLogin([FromBody] LoginDto loginDto)
        {
            string token = await _accountRepository.GenerateJwTAsync(loginDto);
            return Ok(token);
        }
    }
}