using Contact.Domain.Interfaces;
using Contact.Domain.Test.Data_Attribute;
using Contact.Infrastructure.Interfaces;
using Contact.Infrastructure.Service;
using Contact.Shared.Custom;
using Contact.Shared.Dto;
using Contact.Shared.Enum;
using Contact.Shared.Model;
using Microsoft.Extensions.Logging;
using Moq;

namespace Contact.Infrastructure.Test
{
    public class UserTest
    {
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly IUserService _userService;
        private readonly Mock<IJwtService> _jwtServiceMock;
        private readonly Mock<ILogger<UserService>> _loggermock;

        public UserTest()
        {
            _userRepoMock = new Mock<IUserRepository>();
            _jwtServiceMock = new Mock<IJwtService>();
            _loggermock = new Mock<ILogger<UserService>>();
            _userService = new UserService(_userRepoMock.Object, _jwtServiceMock.Object, _loggermock.Object);
        }

        public void Dispose()
        {
        }

        [Theory, UserData]
        public async Task AddUser_SuccessFully(UserDto user)
        {
            await _userService.Register(user);
            _userRepoMock.Verify(x => x.Add(It.IsAny<User>()), Times.Once);
        }

        [Theory, UserData]
        public async Task Login_User_Successfully(LoginDto login, User user)
        {
            _userRepoMock.Setup(x => x.Get(login.Username)).ReturnsAsync(user);
            await _userService.Login(login);

            _jwtServiceMock.Verify(x => x.GenerateSecurityToken(user), Times.Once);
        }

        [Theory, UserData]
        public async Task Login_User_Fail(LoginDto login, User user)
        {
            user.PasswordSalt = new byte[64];
            _userRepoMock.Setup(x => x.Get(login.Username)).ReturnsAsync(user);

            await Assert.ThrowsAsync<UnauthorizedErrorException>(() => _userService.Login(login));
        }

        [Theory, UserData]
        public async Task Add_User_Already_Exists(UserDto user, User user2)
        {
            _userRepoMock.Setup(x => x.Get(user.Username)).ReturnsAsync(user2);
            await Assert.ThrowsAsync<ConflictErrorException>(() => _userService.Register(user));
        }

        [Theory, UserData]
        public async Task Update_User_RoleSuccessfully(User user)
        {
            _userRepoMock.Setup(x => x.Get(user.Username)).ReturnsAsync(user);
            await _userService.UpdateUserRole(user.Username, RoleType.User);
            _userRepoMock.Verify(x => x.Update(It.IsAny<User>()), Times.Once);
        }

        [Theory, UserData]
        public async Task Update_User_Role_Fail(User user)
        {
            _userRepoMock.Setup(x => x.Get(user.Username)).ReturnsAsync((User)null!);
            await Assert.ThrowsAsync<NotFoundErrorException>(() => _userService.UpdateUserRole(user.Username, RoleType.User));
        }

        [Theory, UserData]
        public async Task Update_User_Password_Successfully(User user, ChangePasswordDto changePasswordDto)
        {
            _userRepoMock.Setup(x => x.Get(user.Username)).ReturnsAsync(user);
            await _userService.ChangePassword(user.Username, changePasswordDto);
            _userRepoMock.Verify(x => x.Update(It.IsAny<User>()), Times.Once);
        }

        [Theory, UserData]
        public async Task Update_User_Password_Fail(User user, ChangePasswordDto changePasswordDto)
        {
            _userRepoMock.Setup(x => x.Get(user.Username)).ReturnsAsync((User)null!);
            await Assert.ThrowsAsync<NotFoundErrorException>(() => _userService.ChangePassword(user.Username, changePasswordDto));
        }

        [Theory, UserData]
        public async Task Update_User_Password_Fail_Password(User user, ChangePasswordDto changePasswordDto)
        {
            user.PasswordSalt = new byte[64];
            _userRepoMock.Setup(x => x.Get(user.Username)).ReturnsAsync(user);

            await Assert.ThrowsAsync<UnauthorizedErrorException>(() => _userService.ChangePassword(user.Username, changePasswordDto));
        }

        [Theory, UserData]
        public async Task Delete_User_Role_Successfully(User user)
        {
            _userRepoMock.Setup(x => x.Get(user.Username)).ReturnsAsync(user);
            await _userService.DeleteUser(user.Username);
            _userRepoMock.Verify(x => x.Delete(It.IsAny<User>()), Times.Once);
        }

        [Theory, UserData]
        public async Task Delete_User_Role_Fail(User user)
        {
            _userRepoMock.Setup(x => x.Get(user.Username)).ReturnsAsync((User)null!);
            await Assert.ThrowsAsync<NotFoundErrorException>(() => _userService.DeleteUser(user.Username));
        }

        [Theory, UserData]
        public async Task Delete_User_Successfully(User user)
        {
            _userRepoMock.Setup(x => x.Get(user.Username)).ReturnsAsync(user);
            await _userService.DeleteUser(user.Username);
            _userRepoMock.Verify(x => x.Delete(It.IsAny<User>()), Times.Once);
        }

        [Theory, UserData]
        public async Task Delete_User_Fail(User user)
        {
            _userRepoMock.Setup(x => x.Get(user.Username)).ReturnsAsync((User)null!);
            await Assert.ThrowsAsync<NotFoundErrorException>(() => _userService.DeleteUser(user.Username));
        }
    }
}
