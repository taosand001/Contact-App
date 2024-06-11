using Contact.Domain.Interfaces;
using Contact.Domain.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace Contact.Domain.Extension
{
    public static class ServiceCollectionScope
    {
        public static IServiceCollection AddDomainScope(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPersonalInformationRepository, PersonalInformationRepository>();
            services.AddScoped<IAddressRepository, AddressRepository>();
            return services;
        }
    }
}