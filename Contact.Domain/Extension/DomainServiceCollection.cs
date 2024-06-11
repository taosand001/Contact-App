using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Contact.Domain.Extension
{
    public static class DomainServiceCollection
    {
        public static IServiceCollection AddDomain(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDomainScope();
            services.AddDatabase(configuration);
            return services;
        }
    }
}
