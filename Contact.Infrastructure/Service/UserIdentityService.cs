using Contact.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Contact.Infrastructure.Service
{
    public class UserIdentityService : IUserIdentityService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserIdentityService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUsername()
        {
            return _httpContextAccessor.HttpContext.User.Identity.Name;
        }
    }
}
