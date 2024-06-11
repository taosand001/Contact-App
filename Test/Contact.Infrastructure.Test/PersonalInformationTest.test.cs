using AutoMapper;
using Contact.Domain.Interfaces;
using Contact.Domain.Test.Data_Attribute;
using Contact.Infrastructure.Interfaces;
using Contact.Infrastructure.Mapper;
using Contact.Infrastructure.Service;
using Contact.Shared.Custom;
using Contact.Shared.Dto;
using Contact.Shared.Enum;
using Contact.Shared.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Contact.Infrastructure.Test
{
    public class PersonalInformationTest
    {
        private readonly Mock<IPersonalInformationRepository> _personalInformationRepoMock;
        private readonly Mock<IUserIdentityService> _userIdentityServiceMock;
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly Mock<ILogger<PersonalInformationService>> _loggerMock;
        private readonly IPersonalInformationService _personalInformationService;
        private readonly IMapper _mapper;

        public PersonalInformationTest()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<UserMapper>());
            _mapper = config.CreateMapper();
            _personalInformationRepoMock = new Mock<IPersonalInformationRepository>();
            _userIdentityServiceMock = new Mock<IUserIdentityService>();
            _userRepoMock = new Mock<IUserRepository>();
            _loggerMock = new Mock<ILogger<PersonalInformationService>>();
            _personalInformationService = new PersonalInformationService(_personalInformationRepoMock.Object, _userRepoMock.Object, _loggerMock.Object, _userIdentityServiceMock.Object, _mapper);
        }

        public void Dispose()
        {
        }

        [Theory, PersonalInformationData]
        public async Task AddPersonalInformation_Successfully(PersonalInformationDto personalInformation, User user)
        {
            _userIdentityServiceMock.Setup(x => x.GetUsername()).Returns(user.Username);
            _userRepoMock.Setup(x => x.Get(user.Username)).ReturnsAsync(user);
            await _personalInformationService.AddPersonalInformationAsync(personalInformation);
            _personalInformationRepoMock.Verify(x => x.CreateAsync(It.IsAny<PersonalInformation>()), Times.Once);
        }

        [Theory, PersonalInformationData]
        public async Task AddPersonalInformation_ThrowException(PersonalInformationDto personalInformation, User user)
        {
            _userIdentityServiceMock.Setup(x => x.GetUsername()).Returns(user.Username);
            _userRepoMock.Setup(x => x.Get(user.Username)).ReturnsAsync(user);
            _personalInformationRepoMock.Setup(x => x.CreateAsync(It.IsAny<PersonalInformation>())).ThrowsAsync(new Exception());
            await Assert.ThrowsAsync<Exception>(() => _personalInformationService.AddPersonalInformationAsync(personalInformation));
        }



        [Theory, PersonalInformationData]
        public async Task AddPersonalInformation_UserExists(PersonalInformationDto personalInformation, User user)
        {
            _userIdentityServiceMock.Setup(x => x.GetUsername()).Returns(user.Username);
            _userRepoMock.Setup(x => x.Get(user.Username)).ReturnsAsync(user);
            _personalInformationRepoMock.Setup(x => x.CreateAsync(It.IsAny<PersonalInformation>())).ThrowsAsync(new DbUpdateException("Email already exists"));
            await Assert.ThrowsAsync<Exception>(() => _personalInformationService.AddPersonalInformationAsync(personalInformation));
        }

        [Theory, PersonalInformationData]
        public async Task DeletePersonalInformation_Successfully(User user, PersonalInformation personalInformation)
        {
            _userIdentityServiceMock.Setup(x => x.GetUsername()).Returns(user.Username);
            _userRepoMock.Setup(x => x.Get(user.Username)).ReturnsAsync(user);
            _personalInformationRepoMock.Setup(x => x.GetAsync(personalInformation.Id)).ReturnsAsync(personalInformation);
            await _personalInformationService.DeletePersonalInformationAsync(personalInformation.Id);
            _personalInformationRepoMock.Verify(x => x.DeleteAsync(It.IsAny<PersonalInformation>()), Times.Once);
        }

        [Theory, PersonalInformationData]
        public async Task DeletePersonalInformation_NotFound(User user, int id)
        {
            _userIdentityServiceMock.Setup(x => x.GetUsername()).Returns(user.Username);
            _userRepoMock.Setup(x => x.Get(user.Username)).ReturnsAsync(user);
            _personalInformationRepoMock.Setup(x => x.GetAsync(id)).ReturnsAsync((PersonalInformation)null!);
            await Assert.ThrowsAsync<NotFoundErrorException>(() => _personalInformationService.DeletePersonalInformationAsync(id));
        }

        [Theory, PersonalInformationData]
        public async Task DeletePersonalInformation_User_Not_Found(User user, PersonalInformation personalInformation)
        {
            _userIdentityServiceMock.Setup(x => x.GetUsername()).Returns(user.Username);
            _userRepoMock.Setup(x => x.Get(user.Username)).ReturnsAsync((User)null!);
            _personalInformationRepoMock.Setup(x => x.GetAsync(personalInformation.Id)).ReturnsAsync(personalInformation);
            await Assert.ThrowsAsync<NotFoundErrorException>(() => _personalInformationService.DeletePersonalInformationAsync(personalInformation.Id));
        }

        [Theory, PersonalInformationData]
        public async Task Update_PersonalInformation_Successfully(PersonalInformation personalInformation, PersonalInformationDto personalInformationDto)
        {
            _userIdentityServiceMock.Setup(x => x.GetUsername()).Returns(personalInformation.User!.Username);
            _userRepoMock.Setup(x => x.Get(personalInformation.User!.Username)).ReturnsAsync(personalInformation.User);
            _personalInformationRepoMock.Setup(x => x.GetAsync(personalInformation.Id)).ReturnsAsync(personalInformation);
            await _personalInformationService.UpdatePersonalInformationAsync(personalInformation.Id, personalInformationDto);
            _personalInformationRepoMock.Verify(x => x.UpdateAsync(It.IsAny<PersonalInformation>()), Times.Once);
        }

        [Theory, PersonalInformationData]
        public async Task Update_PersonalInformation_Not_Found(PersonalInformation personalInformation, PersonalInformationDto personalInformationDto)
        {
            _userIdentityServiceMock.Setup(x => x.GetUsername()).Returns(personalInformation.User!.Username);
            _userRepoMock.Setup(x => x.Get(personalInformation.User!.Username)).ReturnsAsync(personalInformation.User);
            _personalInformationRepoMock.Setup(x => x.GetAsync(personalInformation.Id)).ReturnsAsync((PersonalInformation)null!);
            await Assert.ThrowsAsync<NotFoundErrorException>(() => _personalInformationService.UpdatePersonalInformationAsync(personalInformation.Id, personalInformationDto));
        }

        [Theory, PersonalInformationData]
        public async Task Update_PersonalInformation_User_Not_Found(PersonalInformation personalInformation, PersonalInformationDto personalInformationDto)
        {
            _userIdentityServiceMock.Setup(x => x.GetUsername()).Returns(personalInformation.User!.Username);
            _userRepoMock.Setup(x => x.Get(personalInformation.User!.Username)).ReturnsAsync((User)null!);
            _personalInformationRepoMock.Setup(x => x.GetAsync(personalInformation.Id)).ReturnsAsync(personalInformation);
            await Assert.ThrowsAsync<NotFoundErrorException>(() => _personalInformationService.UpdatePersonalInformationAsync(personalInformation.Id, personalInformationDto));
        }

        [Theory, PersonalInformationData]
        public async Task Update_PersonalInformation_User_Not_Authorized(PersonalInformation personalInformation, PersonalInformationDto personalInformationDto)
        {
            var anotherUser = new User
            {
                Username = "anotherUser",
                Role = RoleType.User,
                Id = 2,
            };
            _userIdentityServiceMock.Setup(x => x.GetUsername()).Returns(anotherUser.Username);
            _userRepoMock.Setup(x => x.Get(anotherUser.Username)).ReturnsAsync(anotherUser);
            _personalInformationRepoMock.Setup(x => x.GetAsync(personalInformation.Id)).ReturnsAsync(personalInformation);
            await Assert.ThrowsAsync<UnauthorizedErrorException>(() => _personalInformationService.UpdatePersonalInformationAsync(personalInformation.Id, personalInformationDto));
        }

        [Theory, PersonalInformationData]
        public async Task Update_PersonalInformation_ThrowException(PersonalInformation personalInformation, PersonalInformationDto personalInformationDto)
        {
            _userIdentityServiceMock.Setup(x => x.GetUsername()).Returns(personalInformation.User!.Username);
            _userRepoMock.Setup(x => x.Get(personalInformation.User!.Username)).ReturnsAsync(personalInformation.User);
            _personalInformationRepoMock.Setup(x => x.GetAsync(personalInformation.Id)).ReturnsAsync(personalInformation);
            _personalInformationRepoMock.Setup(x => x.UpdateAsync(It.IsAny<PersonalInformation>())).ThrowsAsync(new DbUpdateException("Info already exists"));
            await Assert.ThrowsAsync<DbUpdateException>(() => _personalInformationService.UpdatePersonalInformationAsync(personalInformation.Id, personalInformationDto));
        }

        [Theory, PersonalInformationData]
        public async Task Update_PersonalInformation_User_Admin_Success(PersonalInformation personalInformation, PersonalInformationDto personalInformationDto)
        {
            var adminUser = new User
            {
                Username = "adminUser",
                Role = RoleType.Admin,
                Id = 2,
            };
            _userIdentityServiceMock.Setup(x => x.GetUsername()).Returns(adminUser.Username);
            _userRepoMock.Setup(x => x.Get(adminUser.Username)).ReturnsAsync(adminUser);
            _personalInformationRepoMock.Setup(x => x.GetAsync(personalInformation.Id)).ReturnsAsync(personalInformation);
            await _personalInformationService.UpdatePersonalInformationAsync(personalInformation.Id, personalInformationDto);
            _personalInformationRepoMock.Verify(x => x.UpdateAsync(It.IsAny<PersonalInformation>()), Times.Once);
        }

        [Theory, PersonalInformationData]
        public async Task Get_PersonalInformation_successfully(User user, PersonalInformation personalInformation)
        {
            _userIdentityServiceMock.Setup(x => x.GetUsername()).Returns(user.Username);
            _userRepoMock.Setup(x => x.Get(user.Username)).ReturnsAsync(user);
            _personalInformationRepoMock.Setup(x => x.GetAsync(personalInformation.Id)).ReturnsAsync(personalInformation);
            var result = await _personalInformationService.GetPersonalInformationAsync(personalInformation.Id);
            Assert.NotNull(result);
        }

        [Theory, PersonalInformationData]
        public async Task Get_PersonalInformation_Not_Found(User user, int id)
        {
            _userIdentityServiceMock.Setup(x => x.GetUsername()).Returns(user.Username);
            _userRepoMock.Setup(x => x.Get(user.Username)).ReturnsAsync(user);
            _personalInformationRepoMock.Setup(x => x.GetAsync(id)).ReturnsAsync((PersonalInformation)null!);
            await Assert.ThrowsAsync<NotFoundErrorException>(() => _personalInformationService.GetPersonalInformationAsync(id));
        }

        [Theory, PersonalInformationData]
        public async Task Get_PersonalInformation_User_Not_Found(User user, PersonalInformation personalInformation)
        {
            _userIdentityServiceMock.Setup(x => x.GetUsername()).Returns(user.Username);
            _userRepoMock.Setup(x => x.Get(user.Username)).ReturnsAsync((User)null!);
            _personalInformationRepoMock.Setup(x => x.GetAsync(personalInformation.Id)).ReturnsAsync(personalInformation);
            await Assert.ThrowsAsync<NotFoundErrorException>(() => _personalInformationService.GetPersonalInformationAsync(personalInformation.Id));
        }

        [Theory, PersonalInformationData]
        public async Task Get_PersonalInformation_User_Not_Authorized(User user, PersonalInformation personalInformation)
        {
            var anotherUser = new User
            {
                Username = "anotherUser",
                Role = RoleType.User,
                Id = 2,
            };
            _userIdentityServiceMock.Setup(x => x.GetUsername()).Returns(anotherUser.Username);
            _userRepoMock.Setup(x => x.Get(anotherUser.Username)).ReturnsAsync(anotherUser);
            _personalInformationRepoMock.Setup(x => x.GetAsync(personalInformation.Id)).ReturnsAsync(personalInformation);
            await Assert.ThrowsAsync<UnauthorizedErrorException>(() => _personalInformationService.GetPersonalInformationAsync(personalInformation.Id));
        }

        [Theory, PersonalInformationData]
        public async Task Get_PersonalInformation_User_Admin_Success(User user, PersonalInformation personalInformation)
        {
            var adminUser = new User
            {
                Username = "adminUser",
                Role = RoleType.Admin,
                Id = 2,
            };
            _userIdentityServiceMock.Setup(x => x.GetUsername()).Returns(adminUser.Username);
            _userRepoMock.Setup(x => x.Get(adminUser.Username)).ReturnsAsync(adminUser);
            _personalInformationRepoMock.Setup(x => x.GetAsync(personalInformation.Id)).ReturnsAsync(personalInformation);
            var result = await _personalInformationService.GetPersonalInformationAsync(personalInformation.Id);
            Assert.NotNull(result);
        }

    }
}
