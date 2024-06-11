using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Contact.Infrastructure.Extension
{
    public static class InfrastructureServiceCollection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper();
            services.AddInfrastructureScope();
            services.AddJwtExtension(configuration);
            services.AddHttpContextAccessor();
            return services;
        }
    }
}
