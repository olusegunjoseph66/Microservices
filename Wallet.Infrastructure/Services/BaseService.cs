using Wallet.Application.Interfaces.Services;

namespace Wallet.Infrastructure.Services
{
    public class BaseService
    {
        internal readonly IAuthenticatedUserService _authenticatedUserService;
        public BaseService(IAuthenticatedUserService authenticatedUserService)
        {
            _authenticatedUserService = authenticatedUserService;
        }

        public int LoggedInUser()
        {
            if (_authenticatedUserService.UserId == 0) throw new UnauthorizedAccessException($"Access Denied.");
            return _authenticatedUserService.UserId;
        }


        internal int GetUserId()
        {
            if (_authenticatedUserService.UserId == 0) throw new UnauthorizedAccessException($"Access Denied.");
            return _authenticatedUserService.UserId;
        }
    }
}
