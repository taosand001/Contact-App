using Contact.Shared.Model;

namespace Contact.Domain.Interfaces
{
    public interface IPersonalInformationRepository
    {
        Task<PersonalInformation> CreateAsync(PersonalInformation personalInformation);
        Task DeleteAsync(PersonalInformation personalInformation);
        Task<IEnumerable<PersonalInformation>> GetAllAsync();
        Task<PersonalInformation> GetAsync(int id);
        Task<PersonalInformation> UpdateAsync(PersonalInformation personalInformation);
        Task<PersonalInformation> GetPersonalCode(string personalCode);
    }
}