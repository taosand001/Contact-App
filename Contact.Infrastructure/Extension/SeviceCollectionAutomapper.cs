using Microsoft.Extensions.DependencyInjection;

namespace Contact.Infrastructure.Extension
{
    public static class SeviceCollectionAutomapper
    {
        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            return services;
        }
    }
}
