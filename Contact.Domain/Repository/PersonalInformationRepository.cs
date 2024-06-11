using Contact.Domain.Database;
using Contact.Domain.Interfaces;
using Contact.Shared.Model;
using Microsoft.EntityFrameworkCore;

namespace Contact.Domain.Repository
{
    public class PersonalInformationRepository : IPersonalInformationRepository
    {
        private readonly DatabaseContext _context;

        public PersonalInformationRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<PersonalInformation> GetAsync(int id)
        {
            return await _context.PersonalInformations.Include(x => x.Address).Include(u => u.User).FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<PersonalInformation> CreateAsync(PersonalInformation personalInformation)
        {
            var result = await _context.PersonalInformations.AddAsync(personalInformation);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<PersonalInformation> UpdateAsync(PersonalInformation personalInformation)
        {
            //_context.Entry(personalInformation).State = EntityState.Modified;
            //var isEmailInUse = await _context.PersonalInformations.AnyAsync(x => x.Email == personalInformation.Email && x.Id != personalInformation.Id);
            //if (isEmailInUse)
            //{
            //    throw new Exception("Email is already in use");
            //}
            var result = _context.PersonalInformations.Update(personalInformation);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task DeleteAsync(PersonalInformation personalInformation)
        {
            _context.PersonalInformations.Remove(personalInformation);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<PersonalInformation>> GetAllAsync()
        {
            return await _context.PersonalInformations.Include(x => x.Address).Include(x => x.User).ToArrayAsync();
        }

    }
}
