using Contact.Shared.Model;

namespace Contact.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task Add(User user);
        Task<User> Get(string user);
        Task Update(User user);
        Task Delete(User user);
    }
}