using AutoFixture;
using AutoFixture.Kernel;
using Contact.Shared.Dto;
using Contact.Shared.Enum;
using Contact.Shared.Model;
using Microsoft.AspNetCore.Http;

namespace Test.Shared.SpecimenBuilder
{
    public class PersonalInformationSpecimenBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (request is Type type && type == typeof(PersonalInformation))
            {
                return new PersonalInformation
                {
                    FirstName = "TestFirstName",
                    LastName = "TestLastName",
                    Email = "test@gmail.com",
                    ImageContentType = "image/jpeg",
                    ImageUrl = "photos/test.jpg",
                    DateOfBirth = DateOnly.FromDateTime(new DateTime(1996, 12, 12)),
                    Gender = GenderType.Male,
                    PersonalCode = "39105141646",
                    User = context.Create<User>(),
                    PhoneNumber = "62659246",
                    Address = context.Create<Address>()
                };
            }

            if (request is Type type2 && type2 == typeof(PersonalInformationDto))
            {
                string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "photos");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                var currentDirectory = Directory.GetCurrentDirectory();
                var filePath = Path.Combine(currentDirectory, "..", "..", "..", "..", "Test.Shared/Pictures/Annotation LT.png");
                var contentBytes = File.ReadAllBytes(filePath);
                var fileStream = new MemoryStream(contentBytes);
                var formFile = new FormFile(fileStream, 0, contentBytes.Length, "Data", "example.png")
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "image/png",
                    ContentDisposition = "form-data; name=\"file\"; filename=\"example.png\""
                };


                return new PersonalInformationDto
                {
                    FirstName = "TestFirstName",
                    LastName = "TestLastName",
                    Email = "test@gmail.com",
                    Image = formFile,
                    DateOfBirth = new DateTime(1996, 12, 12),
                    Gender = GenderType.Male,
                    PersonalCode = "39105141646",
                    PhoneNumber = "62659246",
                    Address = context.Create<AddressDto>()
                };

            }
            return new NoSpecimen();
        }
    }
}
