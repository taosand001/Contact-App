using Contact.Shared.Dto;
using Contact.Shared.Model;

namespace Contact.Infrastructure.Interfaces
{
    public interface IPersonalInformationService
    {
        Task AddPersonalInformationAsync(PersonalInformationDto personalInformationDto);
        Task DeletePersonalInformationAsync(int id);
        Task<IEnumerable<PersonalInformation>> GetAllPersonalInformationAsync();
        Task<PersonalInformation> GetPersonalInformationAsync(int id);
        Task UpdatePersonalInformationAsync(int id, PersonalInformationDto personalInformationDto);
    }
}