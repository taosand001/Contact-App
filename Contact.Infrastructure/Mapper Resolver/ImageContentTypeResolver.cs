using AutoMapper;
using Contact.Shared.Dto;
using Contact.Shared.Model;

namespace Contact.Infrastructure.Mapper_Resolver
{
    public class ImageContentTypeResolver : IValueResolver<PersonalInformationDto, PersonalInformation, string?>
    {
        public string Resolve(PersonalInformationDto source, PersonalInformation destination, string destMember, ResolutionContext context)
        {
            try
            {
                if (source.Image != null && source.Image.Length > 0)
                {
                    return source.Image.ContentType;
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
