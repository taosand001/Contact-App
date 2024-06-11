using Contact.Domain.Extension;
using Contact.Infrastructure.Extension;
using System.Text.Json.Serialization;

namespace Contact.Api.Extension
{
    public static class ServiceCollection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                options.JsonSerializerOptions.WriteIndented = true;
            });
            services.AddSwaggerGen();
            services.AddDomain(configuration);
            services.AddInfrastructure(configuration);

            return services;
        }
    }
}
