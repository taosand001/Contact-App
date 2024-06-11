using Contact.Infrastructure.Interfaces;
using Contact.Infrastructure.Service;
using Microsoft.Extensions.DependencyInjection;

namespace Contact.Infrastructure.Extension
{
    public static class ServiceCollectionScope
    {
        public static IServiceCollection AddInfrastructureScope(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserIdentityService, UserIdentityService>();
            services.AddScoped<IPersonalInformationService, PersonalInformationService>();
            services.AddScoped<IAddressService, AddressService>();
            services.AddTransient<IJwtService, JwtService>();
            return services;
        }
    }
}
