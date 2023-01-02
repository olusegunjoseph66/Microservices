using Wallet.Application.Interfaces.Services;
using System.Security.Claims;

namespace Wallet.API.ClientServices
{
    public class AuthenticatedUserService : IAuthenticatedUserService
    {
        public AuthenticatedUserService(IHttpContextAccessor httpContextAccessor)
        {
            _ = int.TryParse(httpContextAccessor.HttpContext?.User?.FindFirstValue("userId"), out int userKey);
            UserId = userKey;
        }

        public int UserId { get; set; }
    }
}
