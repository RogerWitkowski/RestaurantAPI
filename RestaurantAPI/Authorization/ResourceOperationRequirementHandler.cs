using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Restaurant.Utility;

namespace RestaurantAPI.Authorization
{
    public class ResourceOperationRequirementHandler : AuthorizationHandler<ResourceOperationRequirement, Restaurant.Models.Models.Restaurant>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            ResourceOperationRequirement requirement,
            Restaurant.Models.Models.Restaurant restaurant)
        {
            if (requirement.ResourceOperation == ResourceOperationStaticDetails.Read ||
                requirement.ResourceOperation == ResourceOperationStaticDetails.Create)
            {
                context.Succeed(requirement);
            }

            var userId = context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;

            if (restaurant.CreatedById == int.Parse(userId))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}