using System.Security.Claims;

namespace RestaurantAPI.Repository.IRepository
{
    public interface IUserContextRepository
    {
        ClaimsPrincipal User { get; }
        int? GetUserId { get; }
    }
}