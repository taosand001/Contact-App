using Contact.Domain.Database;
using Contact.Domain.Interfaces;
using Contact.Domain.Repository;
using Contact.Domain.Test.Data_Attribute;
using Contact.Shared.Model;
using Microsoft.EntityFrameworkCore;

namespace Contact.Domain.Test
{
    public class PersonalInformationTest
    {
        private readonly DatabaseContext _context;
        private readonly IPersonalInformationRepository _personalInformationRepository;

        public PersonalInformationTest()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
               .UseInMemoryDatabase(databaseName: "ContactApp")
               .Options;
            _context = new DatabaseContext(options);
            _personalInformationRepository = new PersonalInformationRepository(_context);
        }


        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }


        [Theory, PersonalInformationData]
        public async Task AddPersonalInformation_SuccessFully(PersonalInformation personalInformation)
        {

            await _personalInformationRepository.CreateAsync(personalInformation);

            var result = await _personalInformationRepository.GetAsync(personalInformation.Id);

            Assert.NotNull(result);
            Assert.Equal(personalInformation.Id, result.Id);
        }

        [Theory, PersonalInformationData]
        public async Task GetPersonalInformation_SuccessFully(PersonalInformation personalInformation)
        {

            await _personalInformationRepository.CreateAsync(personalInformation);

            var result = await _personalInformationRepository.GetAsync(personalInformation.Id);

            Assert.NotNull(result);
            Assert.Equal(personalInformation.Id, result.Id);
        }

        [Theory, PersonalInformationData]
        public async Task UpdatePersonalInformation_SuccessFully(PersonalInformation personalInformation)
        {

            await _personalInformationRepository.CreateAsync(personalInformation);

            personalInformation.FirstName = "UpdatedFirstName";
            personalInformation.LastName = "UpdatedLastName";


            await _personalInformationRepository.UpdateAsync(personalInformation);

            var result = await _personalInformationRepository.GetAsync(personalInformation.Id);

            Assert.NotNull(result);
            Assert.Equal(personalInformation.Id, result.Id);
            Assert.Equal(personalInformation.FirstName, result.FirstName);
            Assert.Equal(personalInformation.LastName, result.LastName);
        }

        [Theory, PersonalInformationData]
        public async Task DeletePersonalInformation_SuccessFully(PersonalInformation personalInformation)
        {

            await _personalInformationRepository.CreateAsync(personalInformation);

            await _personalInformationRepository.DeleteAsync(await _personalInformationRepository.GetAsync(personalInformation.Id));

            var result = await _personalInformationRepository.GetAsync(personalInformation.Id);

            Assert.Null(result);
        }


    }
}
