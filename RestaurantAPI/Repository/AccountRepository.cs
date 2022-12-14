using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Restaurant.DataAccess.DataAccess;
using Restaurant.Models.Dto;
using Restaurant.Models.Models;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Repository.IRepository;

namespace RestaurantAPI.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly RestaurantDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IExtensionRepository _extensionRepository;
        private readonly AuthenticationSettings _authenticationSettings;

        public AccountRepository(RestaurantDbContext dbContext, IMapper mapper, IPasswordHasher<User> passwordHasher, AuthenticationSettings authenticationSettings)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _extensionRepository = new ExtensionRepository(dbContext);
            _authenticationSettings = authenticationSettings;
        }

        public async Task<string> GenerateJwTAsync(LoginDto loginDto)
        {
            var user = await _extensionRepository.GetUserWithRoleFromDbByEmailLoginAsync(loginDto);

            var isPasswordCorrect = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginDto.Password);
            if (isPasswordCorrect == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("Invalid username or password!");
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.FirsttName} {user.LasttName}"),
                new Claim(ClaimTypes.Role, $"{user.Role.RoleName}"),
                new Claim("DateOfBirth", user.DateOfBirth.Value.ToString("dd-MM-yyyy")),
            };
            if (!string.IsNullOrEmpty(user.Nationality))
            {
                claims.Add(
                        new Claim("Nationality", user.Nationality)
                    );
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpiryDays);

            var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer,
                _authenticationSettings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: credential);

            var tokenHandler = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenHandler;
        }

        public async Task<ActionResult> RegisterUserAsync(RegisterUserDto registerUserDto)
        {
            var newUser = _mapper.Map<User>(registerUserDto);

            var hashedPassword = _passwordHasher.HashPassword(newUser, registerUserDto.Password);

            newUser.PasswordHash = hashedPassword;
            await _dbContext.Users.AddAsync(newUser);
            await _dbContext.SaveChangesAsync();

            return new OkResult();
        }
    }
}