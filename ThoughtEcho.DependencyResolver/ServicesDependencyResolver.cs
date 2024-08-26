using ThoughtEcho.Services.Implementations;
using ThoughtEcho.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ThoughtEcho.DependencyResolver
{
    public class ServicesDependencyResolver
    {
        public ServicesDependencyResolver(IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEmailService, EmailService>();
        }
    }
}
