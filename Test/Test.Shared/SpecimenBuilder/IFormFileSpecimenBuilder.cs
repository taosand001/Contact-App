using AutoFixture.Kernel;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Text;

namespace Test.Shared.SpecimenBuilder
{
    public class IFormFileSpecimenBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (request is Type type && type == typeof(IFormFile))
            {
                var fileMock = new Mock<IFormFile>();

                string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "photos");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var content = "Example file content";
                var contentBytes = Encoding.UTF8.GetBytes(content);
                var fileStream = new MemoryStream(contentBytes);
                var contentDisposition = "form-data; name=\"file\"; filename=\"example.png\"";
                var formFile = new FormFile(fileStream, 0, contentBytes.Length, "Image/png", "example.png")
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "image/png",
                    ContentDisposition = contentDisposition
                };

                fileMock.Setup(_ => _.FileName).Returns(formFile.FileName);
                fileMock.Setup(_ => _.Length).Returns(formFile.Length);
                fileMock.Setup(_ => _.ContentType).Returns("image/png");

                return fileMock.Object;
            }

            if (request is Type type2 && type2 == typeof(DateOnly))
            {
                var random = new Random();
                var year = random.Next(1900, DateTime.Now.Year);
                var month = random.Next(1, 13);
                var day = random.Next(1, DateTime.DaysInMonth(year, month) + 1);
                return new DateOnly(year, month, day);
            }

            return new NoSpecimen();
        }
    }
}
