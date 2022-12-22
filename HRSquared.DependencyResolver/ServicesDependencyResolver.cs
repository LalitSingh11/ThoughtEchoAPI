using HRSquared.Services.Implementations;
using HRSquared.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace HRSquared.DependencyResolver
{
    public class ServicesDependencyResolver
    {
        public ServicesDependencyResolver(IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();

        }
    }
}
