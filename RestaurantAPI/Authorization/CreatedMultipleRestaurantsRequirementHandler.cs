using Microsoft.AspNetCore.Authorization;
using Restaurant.DataAccess.DataAccess;
using RestaurantAPI.Repository.IRepository;

namespace RestaurantAPI.Authorization
{
    public class CreatedMultipleRestaurantsRequirementHandler : AuthorizationHandler<CreatedMultipleRestaurantsRequirement>
    {
        private readonly RestaurantDbContext _dbContext;
        private readonly IUserContextRepository _userContextRepository;

        public CreatedMultipleRestaurantsRequirementHandler(RestaurantDbContext dbContext, IUserContextRepository userContextRepository)
        {
            _dbContext = dbContext;
            _userContextRepository = userContextRepository;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CreatedMultipleRestaurantsRequirement requirement)
        {
            var userId = _userContextRepository.GetUserId;

            var createdRestaurantCount = _dbContext
                .Restaurants
                .Count(r => r.CreatedById == userId);

            if (createdRestaurantCount >= requirement.MinimumRestaurantCreated)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}