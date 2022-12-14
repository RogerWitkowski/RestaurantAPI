using Microsoft.AspNetCore.Authorization;
using Restaurant.Utility;

namespace RestaurantAPI.Authorization
{
    public class ResourceOperationRequirement : IAuthorizationRequirement
    {
        public ResourceOperationRequirement(ResourceOperationStaticDetails resourceOperation)
        {
            ResourceOperation = resourceOperation;
        }

        public ResourceOperationStaticDetails ResourceOperation { get; set; }
    }
}