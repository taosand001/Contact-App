using Contact.Infrastructure.Helpers;
using Contact.Shared.Custom;

namespace Contact.Api.Extension
{
    public static class AppSettingsExtension
    {
        public static void ConfigureAppSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppSettings>(configuration.GetSection("AppSettings"));
            var appSettings = configuration.GetSection("AppSettings").Get<AppSettings>();
            Vaildations.SetAllowedDomains(appSettings!.AllowedDomains);
            EmailService.InitializeEmailClient(appSettings!.EmailClient);
        }
    }
}
