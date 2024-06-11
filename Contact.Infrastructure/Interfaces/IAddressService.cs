using Contact.Shared.Dto;

namespace Contact.Infrastructure.Interfaces
{
    public interface IAddressService
    {
        Task CreateAddressAsync(CreateAddressDto address);
    }
}