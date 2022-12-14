﻿using System.Security.Claims;
using RestaurantAPI.Repository.IRepository;

namespace RestaurantAPI.Repository
{
    public class UserContextRepository : IUserContextRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContextRepository(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public ClaimsPrincipal User => _httpContextAccessor.HttpContext?.User;

        public int? GetUserId => User is null ? null : int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
    }
}