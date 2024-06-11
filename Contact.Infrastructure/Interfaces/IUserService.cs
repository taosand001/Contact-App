
using Contact.Shared.Dto;
using Contact.Shared.Enum;

namespace Contact.Infrastructure.Interfaces
{
    public interface IUserService
    {
        Task<string> Login(LoginDto user);
        Task<string> Register(UserDto user);
        Task UpdateUserRole(string userName, RoleType user);
        Task DeleteUserAdminRole(string userName);
        Task ChangePassword(string userName, ChangePasswordDto changePassword);
        Task DeleteUser(string userName);
        Task SendPasswordToken(string userName);
        Task VerifyPasswordToken(string userName, VerifyPasswordDto verifyPasswordDto);
    }
}