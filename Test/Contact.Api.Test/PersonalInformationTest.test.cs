using Contact.Api.Controllers;
using Contact.Domain.Test.Data_Attribute;
using Contact.Infrastructure.Interfaces;
using Contact.Shared.Custom;
using Contact.Shared.Dto;
using Contact.Shared.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Moq;
using Newtonsoft.Json;
using System.Security.Claims;
using Test.Shared.Faker;

namespace Contact.Api.Test
{
    public class PersonalInformationTest
    {
        private readonly Mock<IPersonalInformationService> _personalInformationServiceMock;
        private readonly Mock<IUserIdentityService> _userIdentityMock;
        private readonly PersonalInformationController _personalInformationController;

        public PersonalInformationTest()
        {
            _personalInformationServiceMock = new Mock<IPersonalInformationService>();
            _userIdentityMock = new Mock<IUserIdentityService>();


            var routeData = new RouteData();
            var httpContext = new DefaultHttpContext();
            routeData.Values.Add("controller", "PersonalInformation");
            var actionContext = new ActionContext(httpContext, routeData, new ControllerActionDescriptor());
            _personalInformationController = new PersonalInformationController(_personalInformationServiceMock.Object)
            {
                Url = new UrlHelper(actionContext),
            };

            var mockClaimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
              {
                    new Claim(ClaimTypes.Name, "TestUser")
              }));

            _personalInformationController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = mockClaimsPrincipal }
            };

            var urlHelper = new FakeUrlHelper("http://localhost:5000/api/PersonalInformation/GetImage/1");
            _personalInformationController.Url = urlHelper;
        }

        [Theory, PersonalInformationData]
        public async Task AddPersonalInformation_Success(PersonalInformationDto personalInformation, User user)
        {
            var response = new { message = "Personal information has been created" };

            _userIdentityMock.Setup(x => x.GetUsername()).Returns(user.Username);
            _personalInformationServiceMock.Setup(x => x.AddPersonalInformationAsync(personalInformation)).Returns(Task.CompletedTask);

            var result = await _personalInformationController.AddPersonalInformation(personalInformation) as ObjectResult;

            var actualResponseJson = JsonConvert.SerializeObject(result!.Value);
            var expectedResponseJson = JsonConvert.SerializeObject(response);

            Assert.IsType<CreatedResult>(result);
            Assert.Equal(expectedResponseJson, actualResponseJson);
        }

        [Theory, PersonalInformationData]
        public async Task AddPersonalInformation_Fail(PersonalInformationDto personalInformation, User user)
        {
            _userIdentityMock.Setup(x => x.GetUsername()).Returns(user.Username);
            _personalInformationServiceMock.Setup(x => x.AddPersonalInformationAsync(personalInformation)).Throws(new NotFoundErrorException("User not found"));

            var result = await _personalInformationController.AddPersonalInformation(personalInformation) as BadRequestObjectResult;

            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("User not found", result.Value);
        }

        [Theory, PersonalInformationData]
        public async Task AddPersonalInformation_Conflict(PersonalInformationDto personalInformation, User user)
        {
            _userIdentityMock.Setup(x => x.GetUsername()).Returns(user.Username);
            _personalInformationServiceMock.Setup(x => x.AddPersonalInformationAsync(personalInformation)).Throws(new ConflictErrorException("Personal Information could not be saved. Email already exists"));

            var result = await _personalInformationController.AddPersonalInformation(personalInformation) as BadRequestObjectResult;

            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Personal Information could not be saved. Email already exists", result.Value);
        }

        [Theory, PersonalInformationData]
        public async Task AddPersonalInformation_InternalServerError(PersonalInformationDto personalInformation, User user)
        {
            _userIdentityMock.Setup(x => x.GetUsername()).Returns(user.Username);
            _personalInformationServiceMock.Setup(x => x.AddPersonalInformationAsync(personalInformation)).Throws(new Exception("Internal Server Error"));

            var result = await _personalInformationController.AddPersonalInformation(personalInformation) as ObjectResult;

            Assert.IsType<ObjectResult>(result);
            Assert.Equal("Internal Server Error", result.Value);
        }

        [Theory, PersonalInformationData]
        public async Task UpdatePersonalInformation_Success(PersonalInformationDto personalInformation, int id)
        {
            var response = new { message = "Personal information has been updated" };

            _personalInformationServiceMock.Setup(x => x.UpdatePersonalInformationAsync(id, personalInformation)).Returns(Task.CompletedTask);

            var result = await _personalInformationController.UpdatePersonalInformation(id, personalInformation) as OkObjectResult;

            var actualResponseJson = JsonConvert.SerializeObject(result!.Value);
            var expectedResponseJson = JsonConvert.SerializeObject(response);

            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedResponseJson, actualResponseJson);
        }

        [Theory, PersonalInformationData]
        public async Task UpdatePersonalInformation_Fail(PersonalInformationDto personalInformation, int id)
        {
            _personalInformationServiceMock.Setup(x => x.UpdatePersonalInformationAsync(id, personalInformation)).Throws(new NotFoundErrorException("Personal Information not found"));

            var result = await _personalInformationController.UpdatePersonalInformation(id, personalInformation) as BadRequestObjectResult;

            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Personal Information not found", result.Value);
        }

        [Theory, PersonalInformationData]
        public async Task UpdatePersonalInformation_InternalServerError(PersonalInformationDto personalInformation, int id)
        {
            _personalInformationServiceMock.Setup(x => x.UpdatePersonalInformationAsync(id, personalInformation)).Throws(new Exception("Internal Server Error"));

            var result = await _personalInformationController.UpdatePersonalInformation(id, personalInformation) as ObjectResult;

            Assert.IsType<ObjectResult>(result);
            Assert.Equal("Internal Server Error", result.Value);
        }

        [Theory, PersonalInformationData]
        public async Task DeletePersonalInformation_Success(int id, User user)
        {
            var response = new { message = "Personal information has been deleted" };

            _userIdentityMock.Setup(x => x.GetUsername()).Returns(user.Username);
            _personalInformationServiceMock.Setup(x => x.DeletePersonalInformationAsync(id)).Returns(Task.CompletedTask);

            var result = await _personalInformationController.DeletePersonalInformation(id) as OkObjectResult;

            var actualResponseJson = JsonConvert.SerializeObject(result!.Value);
            var expectedResponseJson = JsonConvert.SerializeObject(response);

            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedResponseJson, actualResponseJson);
        }

        [Theory, PersonalInformationData]
        public async Task DeletePersonalInformation_Fail(int id, User user)
        {
            _userIdentityMock.Setup(x => x.GetUsername()).Returns(user.Username);
            _personalInformationServiceMock.Setup(x => x.DeletePersonalInformationAsync(id)).Throws(new NotFoundErrorException("Personal Information not found"));

            var result = await _personalInformationController.DeletePersonalInformation(id) as BadRequestObjectResult;

            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Personal Information not found", result.Value);
        }

        [Theory, PersonalInformationData]
        public async Task DeletePersonalInformation_InternalServerError(int id, User user)
        {
            _userIdentityMock.Setup(x => x.GetUsername()).Returns(user.Username);
            _personalInformationServiceMock.Setup(x => x.DeletePersonalInformationAsync(id)).Throws(new Exception("Internal Server Error"));

            var result = await _personalInformationController.DeletePersonalInformation(id) as ObjectResult;

            Assert.IsType<ObjectResult>(result);
            Assert.Equal("Internal Server Error", result.Value);
        }

        [Theory, PersonalInformationData]
        public async Task GetPersonalInformation_Success(User user, PersonalInformation personalInformation)
        {
            _userIdentityMock.Setup(x => x.GetUsername()).Returns(user.Username);
            _personalInformationServiceMock.Setup(x => x.GetPersonalInformationAsync(It.IsAny<int>())).ReturnsAsync(personalInformation);

            var result = await _personalInformationController.GetPersonalInformation(It.IsAny<int>()) as OkObjectResult;

            var response = result!.Value as PersonalInformationResponseDto;

            Assert.IsType<OkObjectResult>(result);
            Assert.Equal("http://localhost:5000/api/PersonalInformation/GetImage/1", response!.ImageUrl);
        }

        [Theory, PersonalInformationData]
        public async Task GetPersonalInformation_Fail(int id, User user)
        {
            _userIdentityMock.Setup(x => x.GetUsername()).Returns(user.Username);
            _personalInformationServiceMock.Setup(x => x.GetPersonalInformationAsync(id)).Throws(new NotFoundErrorException("Personal Information not found"));

            var result = await _personalInformationController.GetPersonalInformation(id) as BadRequestObjectResult;

            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Personal Information not found", result.Value);
        }

        [Theory, PersonalInformationData]
        public async Task GetPersonalInformation_InternalServerError(int id, User user)
        {
            _userIdentityMock.Setup(x => x.GetUsername()).Returns(user.Username);
            _personalInformationServiceMock.Setup(x => x.GetPersonalInformationAsync(id)).Throws(new Exception("Internal Server Error"));

            var result = await _personalInformationController.GetPersonalInformation(id) as ObjectResult;

            Assert.IsType<ObjectResult>(result);
            Assert.Equal("Internal Server Error", result.Value);
        }

        [Theory, PersonalInformationData]
        public async Task GetAllPersonalInformation_Success(User user)
        {
            var personalInformation = new List<PersonalInformation>
            {
                new PersonalInformation
                {
                    User = user
                }
            };

            _userIdentityMock.Setup(x => x.GetUsername()).Returns(user.Username);
            _personalInformationServiceMock.Setup(x => x.GetAllPersonalInformationAsync()).ReturnsAsync(personalInformation);

            var result = await _personalInformationController.GetAllPersonalInformation() as OkObjectResult;

            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(personalInformation, result.Value);
        }

        [Fact]
        public async Task Get_PersonalInformation_User_Unauthorized()
        {
            _personalInformationServiceMock.Setup(x => x.GetPersonalInformationAsync(It.IsAny<int>())).ThrowsAsync(new UnauthorizedErrorException("User not authorized to delete this personal information"));

            var result = await _personalInformationController.GetPersonalInformation(It.IsAny<int>());

            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("User not authorized to delete this personal information", ((BadRequestObjectResult)result).Value);
        }

        [Fact]
        public async Task Get_PersonalInformation_User_NotFound()
        {
            _personalInformationServiceMock.Setup(x => x.GetPersonalInformationAsync(It.IsAny<int>())).ThrowsAsync(new NotFoundErrorException("User not found"));

            var result = await _personalInformationController.GetPersonalInformation(It.IsAny<int>());

            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("User not found", ((BadRequestObjectResult)result).Value);
        }
    }
}
