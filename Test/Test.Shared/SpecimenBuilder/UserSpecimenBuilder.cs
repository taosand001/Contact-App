using AutoFixture.Kernel;
using Contact.Shared.Dto;
using Contact.Shared.Enum;
using Contact.Shared.Model;
using System.Security.Cryptography;
using System.Text;

namespace Test.Shared.SpecimenBuilder
{
    public class UserSpecimenBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (request is Type type && type == typeof(User))
            {
                //static method that hashes password(PasswordHashingService right now) TestPassword
                var password = "TestPassword";
                using var hmac = new HMACSHA512();
                var passwordSalt = hmac.Key;
                var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return new User
                {
                    Username = "TestUser",
                    Email = "test@gmail.com",
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    Role = RoleType.User,
                    PersonalInformation = new List<PersonalInformation>()
                };

            }

            if (request is Type type2 && type2 == typeof(UserDto))
            {
                return new UserDto
                {
                    Username = "TestUser",
                    Email = "test@gmail.com",
                    Password = "TestPassword",
                };
            }

            if (request is Type type3 && type3 == typeof(LoginDto))
            {
                return new LoginDto(Username: "TestUser", Password: "TestPassword");
            }

            if (request is Type type4 && type4 == typeof(ChangePasswordDto))
            {
                return new ChangePasswordDto(oldPassword: "TestPassword", newPassword: "NewPassword");
            }
            return new NoSpecimen();
        }
    }
}
