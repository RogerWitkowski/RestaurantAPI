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
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterUser([FromBody] RegisterUserDto registerUserDto)
        {
            var newUser = await _accountRepository.RegisterUserAsync(registerUserDto);
            return newUser;
        }
    }
}