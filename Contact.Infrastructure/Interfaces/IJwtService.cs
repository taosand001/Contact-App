
using Contact.Shared.Model;

namespace Contact.Infrastructure.Interfaces
{
    public interface IJwtService
    {
        string GenerateSecurityToken(User user);
    }
}