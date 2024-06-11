using AutoMapper;
using Contact.Infrastructure.Mapper_Resolver;
using Contact.Shared.Dto;
using Contact.Shared.Model;

namespace Contact.Infrastructure.Mapper
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<UserDto, User>().ReverseMap();
            CreateMap<PersonalInformationDto, PersonalInformation>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom<ImagePathResolver>())
                .ForMember(dest => dest.ImageContentType, opt => opt.MapFrom<ImageContentTypeResolver>())
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.DateOfBirth)));

            CreateMap<AddressDto, Address>().ReverseMap();
            CreateMap<CreateAddressDto, Address>().ReverseMap();
        }
    }
}
