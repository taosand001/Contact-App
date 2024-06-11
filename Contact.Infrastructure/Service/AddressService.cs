using AutoMapper;
using Contact.Domain.Interfaces;
using Contact.Infrastructure.Interfaces;
using Contact.Shared.Custom;
using Contact.Shared.Dto;
using Contact.Shared.Model;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Contact.Infrastructure.Service
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;
        private readonly ILogger<AddressService> _logger;
        private readonly IMapper _mapper;

        public AddressService(IAddressRepository addressRepository, ILogger<AddressService> logger, IMapper mapper)
        {
            _addressRepository = addressRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task CreateAddressAsync(CreateAddressDto address)
        {
            try
            {
                var newAddress = _mapper.Map<Address>(address);
                await _addressRepository.AddAsync(newAddress);
                _logger.LogInformation("Address created successfully");
            }
            catch (DbUpdateException ex) when (IsUniqueConstraintViolation(ex))
            {
                _logger.LogError(ex, "Address already exists for this personal information {personalInfo}", address.PersonalInformationId);
                throw new ConflictErrorException("Address already exists for this personal information");
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
