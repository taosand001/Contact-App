using AutoMapper;
using Contact.Domain.Interfaces;
using Contact.Infrastructure.Helpers;
using Contact.Infrastructure.Interfaces;
using Contact.Shared.Custom;
using Contact.Shared.Dto;
using Contact.Shared.Enum;
using Contact.Shared.Model;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Contact.Infrastructure.Service
{
    public class PersonalInformationService : IPersonalInformationService
    {
        private readonly IPersonalInformationRepository _personalInformationRepository;
        private readonly ILogger<PersonalInformationService> _logger;
        private readonly IUserIdentityService _identityService;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public PersonalInformationService(
            IPersonalInformationRepository personalInformationRepository,
            IUserRepository userRepository,
            ILogger<PersonalInformationService> logger,
            IUserIdentityService identityService,
            IMapper mapper)
        {
            _personalInformationRepository = personalInformationRepository;
            _logger = logger;
            _identityService = identityService;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task AddPersonalInformationAsync(PersonalInformationDto personalInformationDto)
        {
            try
            {
                var username = _identityService.GetUsername();
                var user = await _userRepository.Get(username);
                if (user == null)
                {
                    throw new NotFoundErrorException("User not found");
                }

                var (personalCodeBirthDate, gender) = PersonalCodeExtractor.ExtractBirthDateFromPersonalCode(personalInformationDto.PersonalCode);

                if (gender != personalInformationDto.Gender)
                {
                    throw new ArgumentException("The gender in the personal code does not match the provided gender.");
                }

                if (personalCodeBirthDate.Year != personalInformationDto.DateOfBirth.Year ||
                    personalCodeBirthDate.Month != personalInformationDto.DateOfBirth.Month)
                {
                    throw new ArgumentException("The birth date in the personal code does not match the provided birth date.");
                }

                var existingPersonalCode = await _personalInformationRepository.GetPersonalCode(personalInformationDto.PersonalCode);
                if (existingPersonalCode != null)
                {
                    throw new ConflictErrorException("Personal Information could not be saved. Personal Code already exists");
                }

                var personalInformation = _mapper.Map<PersonalInformation>(personalInformationDto);
                personalInformation.User = user;

                await _personalInformationRepository.CreateAsync(personalInformation);
            }
            catch (DbUpdateException ex) when (IsUniqueConstraintViolation(ex))
            {
                _logger.LogError(ex, "Personal Information could not be saved. Email already exists");
                throw new ConflictErrorException("Personal Information could not be saved. Email already exists");
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new ArgumentException(ex.Message.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new Exception(ex.Message.ToString());
            }
        }

        public async Task DeletePersonalInformationAsync(int id)
        {

            var username = _identityService.GetUsername();
            var user = await _userRepository.Get(username);
            if (user == null)
            {
                throw new NotFoundErrorException("User not found");
            }

            var personalInformation = await _personalInformationRepository.GetAsync(id);

            if (personalInformation == null)
            {
                throw new NotFoundErrorException("Personal Information not found");
            }
            if (user.Id != personalInformation.User!.Id && user.Role is not RoleType.Admin)
            {
                _logger.LogError("User not authorized to delete this personal information: your username {username}: personal Info username {personalUsername}", user.Username, personalInformation.User.Username);
                throw new UnauthorizedErrorException("User not authorized to delete this personal information");
            }

            await _personalInformationRepository.DeleteAsync(personalInformation);
        }

        public async Task<IEnumerable<PersonalInformation>> GetAllPersonalInformationAsync()
        {
            try
            {
                var username = _identityService.GetUsername();
                var user = await _userRepository.Get(username);
                if (user == null)
                {
                    throw new NotFoundErrorException("User not found");
                }
                if (user.Role is RoleType.Admin)
                {
                    return await _personalInformationRepository.GetAllAsync();
                }
                var personalInformations = await _personalInformationRepository.GetAllAsync();
                return personalInformations.Where(x => x.User is not null && x.User!.Id == user.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new Exception(ex.Message.ToString());
            }
        }

        public async Task<PersonalInformation> GetPersonalInformationAsync(int id)
        {

            var username = _identityService.GetUsername();
            var user = await _userRepository.Get(username);
            if (user == null)
            {
                throw new NotFoundErrorException("User not found");
            }

            var personalInformation = await _personalInformationRepository.GetAsync(id);
            if (personalInformation == null)
            {
                throw new NotFoundErrorException("Personal Information not found");
            }
            if (user.Id != personalInformation.User!.Id && user.Role is not RoleType.Admin)
            {
                _logger.LogError("User not authorized to get this personal information: your username {username}: personal Info username {personalUsername}", user.Username, personalInformation.User.Username);
                throw new UnauthorizedErrorException("User not authorized to get this personal information");
            }

            return personalInformation;
        }

        public async Task UpdatePersonalInformationAsync(int id, PersonalInformationDto personalInformationDto)
        {
            try
            {
                var username = _identityService.GetUsername();
                var user = await _userRepository.Get(username);
                if (user == null)
                {
                    throw new NotFoundErrorException("User not found");
                }

                var personalInformation = await _personalInformationRepository.GetAsync(id);
                if (personalInformation == null)
                {
                    throw new NotFoundErrorException("Personal Information not found");
                }
                if (user.Id != personalInformation.User!.Id && user.Role is not RoleType.Admin)
                {
                    _logger.LogError("User not authorized to update this personal information: your username {username}: personal Info username {personalUsername}", user.Username, personalInformation.User.Username);
                    throw new UnauthorizedErrorException("User not authorized to update this personal information");
                }

                ImageService.DeleteImage(personalInformation.ImageUrl!);
                var updatedPersonalInformation = _mapper.Map(personalInformationDto, personalInformation);
                if (personalInformationDto.Address is not null)
                {
                    updatedPersonalInformation.Address = _mapper.Map(personalInformationDto.Address, personalInformation.Address);
                }
                await _personalInformationRepository.UpdateAsync(personalInformation);
            }
            catch (DbUpdateException ex) when (IsUniqueConstraintViolation(ex))
            {
                _logger.LogError(ex, "Personal Information could not be saved. Email already exists");
                throw new Exception("Personal Information could not be saved. Email already exists");
            }
        }

        private bool IsUniqueConstraintViolation(DbUpdateException ex)
        {
            if (ex.InnerException is SqlException sqlException)
            {
                return sqlException.Number == 2601 || sqlException.Number == 2627;
            }
            return false;
        }

    }
}
