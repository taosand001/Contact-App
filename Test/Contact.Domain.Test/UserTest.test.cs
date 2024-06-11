using Contact.Domain.Database;
using Contact.Domain.Interfaces;
using Contact.Domain.Repository;
using Contact.Domain.Test.Data_Attribute;
using Contact.Shared.Enum;
using Contact.Shared.Model;
using Microsoft.EntityFrameworkCore;



namespace Contact.Domain.Test
{
    public class UserTest
    {
        private readonly DatabaseContext _context;
        private readonly IUserRepository _userRepository;

        public UserTest()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
               .UseInMemoryDatabase(databaseName: "ContactApp")
               .Options;
            _context = new DatabaseContext(options);
            _userRepository = new UserRepository(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Theory, UserData]
        public async Task AddUser_SuccessFully(User user)
        {

            await _userRepository.Add(user);

            var result = await _userRepository.Get(user.Username);

            Assert.NotNull(result);
            Assert.Equal(user.Username, result.Username);
        }

        [Theory, UserData]
        public async Task GetUser_SuccessFully(User user)
        {

            await _userRepository.Add(user);

            var result = await _userRepository.Get(user.Username);

            Assert.NotNull(result);
            Assert.Equal(user.Username, result.Username);
        }

        [Theory, UserData]
        public async Task UpdateUser_SuccessFully(User user)
        {

            await _userRepository.Add(user);

            var result = await _userRepository.Get(user.Username);

            result.Username = "TestUserUpdated";

            await _userRepository.Update(result);

            var updatedResult = await _userRepository.Get("TestUserUpdated");

            Assert.NotNull(updatedResult);
            Assert.Equal("TestUserUpdated", updatedResult.Username);
        }

        [Theory, UserData]
        public async Task UpdateUserRole(User user)
        {

            await _userRepository.Add(user);

            var result = await _userRepository.Get(user.Username);

            result.Role = RoleType.Admin;

            await _userRepository.Update(result);

            var updatedResult = await _userRepository.Get(user.Username);

            Assert.NotNull(updatedResult);
            Assert.Equal("Admin", updatedResult.Role.ToString());
        }
    }
}