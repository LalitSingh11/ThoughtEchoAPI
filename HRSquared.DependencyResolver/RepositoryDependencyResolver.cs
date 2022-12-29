using Microsoft.Extensions.DependencyInjection;
using HRSquared.Repository.Implementations;
using HRSquared.Repository.Interfaces;

namespace HRSquared.DependencyResolver
{
    public class RepositoryDependencyResolver
    {
        public RepositoryDependencyResolver(IServiceCollection services) 
        {
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}
