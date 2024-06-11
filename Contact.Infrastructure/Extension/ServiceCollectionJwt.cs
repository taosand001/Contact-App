using Contact.Domain.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Contact.Infrastructure.Extension
{
    public static class ServiceCollectionJwt
    {
        public static IServiceCollection AddJwtExtension(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            {
                                context.Response.Headers.Add("Token-Expired", "true");
                            }
                            return Task.CompletedTask;
                        },

                        OnTokenValidated = async context =>
                        {
                            var userService = context.HttpContext.RequestServices.GetRequiredService<IUserRepository>();
                            var userName = context.Principal.Identity.Name;
                            var lastPasswordChangeFromToken = DateTime.Parse(context.Principal.FindFirst("LastPasswordChange").Value);
                            var user = await userService.Get(userName);
                            if (user == null || DateTime.Parse(user.LastPasswordChange.ToString()) > lastPasswordChangeFromToken)
                            {
                                context.Fail("Invalid Token");
                            }
                        }
                    };
                });
            return services;
        }
    }
}
