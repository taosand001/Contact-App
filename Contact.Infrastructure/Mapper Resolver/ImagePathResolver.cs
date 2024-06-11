using AutoMapper;
using Contact.Shared.Dto;
using Contact.Shared.Model;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Contact.Infrastructure.Mapper_Resolver
{
    public class ImagePathResolver : IValueResolver<PersonalInformationDto, PersonalInformation, string?>
    {
        public string Resolve(PersonalInformationDto source, PersonalInformation destination, string destMember, ResolutionContext context)
        {
            try
            {
                if (source.Image != null && source.Image.Length > 0)
                {
                    string folderName = "photos";
                    string folderPath = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    string filePath = $"{folderName}/{source.Image.FileName}_{DateTime.Now.ToString("yyyy_MM_dd_H_m_s")}{Path.GetExtension(source.Image.FileName)}";
                    using (var image = Image.Load(source.Image.OpenReadStream()))
                    {
                        image.Mutate(x => x.Resize(200, 200));
                        image.Save(filePath);
                    }
                    return filePath;
                }
                return destination.ImageUrl!;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
