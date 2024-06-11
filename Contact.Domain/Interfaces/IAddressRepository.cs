using Contact.Shared.Model;

namespace Contact.Domain.Interfaces
{
    public interface IAddressRepository
    {
        Task AddAsync(Address address);
        Task DeleteAsync(Address address);
        Task<IEnumerable<Address>> GetAllAsync();
        Task<Address> GetAsync(int id);
        Task UpdateAsync(Address address);
    }
}