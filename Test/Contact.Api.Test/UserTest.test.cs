using Contact.Api.Controllers;
using Contact.Domain.Test.Data_Attribute;
using Contact.Infrastructure.Interfaces;
using Contact.Shared.Custom;
using Contact.Shared.Dto;
using Contact.Shared.Enum;
using Contact.Shared.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Contact.Api.Test
{
    public class UserTest
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly UserController _userController;

        public UserTest()
        {
            _userServiceMock = new Mock<IUserService>();
            _userController = new UserController(_userServiceMock.Object);

            var mockClaimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
               {
                    new Claim(ClaimTypes.Name, "TestUser")
               }));

            _userController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = mockClaimsPrincipal }
            };
        }

        [Theory, UserData]
        public async Task LoginUser_Success(LoginDto loginDto)
        {
            _userServiceMock.Setup(x => x.Login(loginDto)).ReturnsAsync("token");

            var result = await _userController.Login(loginDto);

            Assert.IsType<OkObjectResult>(result);
            Assert.Equal("token", ((ObjectResult)result).Value);
        }

        [Theory, UserData]
        public async Task LoginUser_Fail(LoginDto loginDto)
        {
            _userServiceMock.Setup(x => x.Login(loginDto)).ThrowsAsync(new UnauthorizedErrorException("Username or password is incorrect"));

            var result = await _userController.Login(loginDto);

            Assert.IsType<ObjectResult>(result);
            Assert.Equal("Username or password is incorrect", ((ObjectResult)result).Value);
        }

        [Theory, UserData]
        public async Task RegisterUser_Success(UserDto userDto)
        {
            var response = new { message = "User has been created", token = "token" };
            _userServiceMock.Setup(x => x.Register(userDto)).ReturnsAsync("token");

            var result = await _userController.Register(userDto) as ObjectResult;

            var actualResponseJson = JsonConvert.SerializeObject(result!.Value);
            var expectedResponseJson = JsonConvert.SerializeObject(response);

            Assert.IsType<CreatedResult>(result);
            Assert.Equal(expectedResponseJson, actualResponseJson);
        }

        [Theory, UserData]
        public async Task RegisterUser_Fail(UserDto userDto)
        {
            _userServiceMock.Setup(x => x.Register(userDto)).ThrowsAsync(new ConflictErrorException("User already exists"));

            var result = await _userController.Register(userDto);

            Assert.IsType<ObjectResult>(result);
            Assert.True(((ObjectResult)result).StatusCode == 409);
            Assert.Equal("User already exists", ((ObjectResult)result).Value);
        }

        [Theory, UserData]
        public async Task UpdateRole_Success(User user)
        {
            _userServiceMock.Setup(x => x.UpdateUserRole(user.Username, RoleType.Admin)).Returns(Task.CompletedTask);

            var result = await _userController.UpdateUserRole(user.Username, RoleType.Admin);

            Assert.IsType<OkObjectResult>(result);
        }

        [Theory, UserData]
        public async Task UpdateRole_Fail(User user)
        {
            _userServiceMock.Setup(x => x.UpdateUserRole(user.Username, RoleType.Admin)).Throws(new NotFoundErrorException("User not found"));

            var result = await _userController.UpdateUserRole(user.Username, RoleType.Admin);

            Assert.IsType<ObjectResult>(result);
            Assert.Equal("User not found", ((ObjectResult)result).Value);
        }

        [Theory, UserData]
        public async Task DeleteUserAdminRole_Success(User user)
        {
            _userServiceMock.Setup(x => x.DeleteUserAdminRole(user.Username)).Returns(Task.CompletedTask);

            var result = await _userController.DeleteUserAdminRole(user.Username);

            Assert.IsType<OkObjectResult>(result);
        }

        [Theory, UserData]
        public async Task DeleteUserAdminRole_Fail(User user)
        {
            _userServiceMock.Setup(x => x.DeleteUserAdminRole(user.Username)).Throws(new NotFoundErrorException("User not found"));

            var result = await _userController.DeleteUserAdminRole(user.Username);

            Assert.IsType<ObjectResult>(result);
            Assert.Equal("User not found", ((ObjectResult)result).Value);
        }

        [Theory, UserData]
        public async Task DeleteUser_Success(User user)
        {
            _userServiceMock.Setup(x => x.DeleteUser(user.Username)).Returns(Task.CompletedTask);

            var result = await _userController.DeleteUser(user.Username);

            Assert.IsType<OkObjectResult>(result);
        }

        [Theory, UserData]
        public async Task DeleteUser_Fail(User user)
        {
            _userServiceMock.Setup(x => x.DeleteUser(user.Username)).Throws(new NotFoundErrorException("User not found"));

            var result = await _userController.DeleteUser(user.Username);

            Assert.IsType<ObjectResult>(result);
            Assert.Equal("User not found", ((ObjectResult)result).Value);
        }

        [Theory, UserData]
        public async Task ChangePassword_Success(User user, ChangePasswordDto changePasswordDto)
        {
            _userServiceMock.Setup(x => x.ChangePassword(user.Username, changePasswordDto)).Returns(Task.CompletedTask);

            var result = await _userController.ChangePassword(changePasswordDto);

            Assert.IsType<OkObjectResult>(result);
        }

        [Theory, UserData]
        public async Task ChangePassword_Fail(User user, ChangePasswordDto changePasswordDto)
        {
            _userServiceMock.Setup(x => x.ChangePassword(user.Username, changePasswordDto)).Throws(new NotFoundErrorException("User not found"));

            var result = await _userController.ChangePassword(changePasswordDto);

            Assert.IsType<ObjectResult>(result);
            Assert.Equal("User not found", ((ObjectResult)result).Value);
        }
    }
}
