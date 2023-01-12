using Microsoft.Extensions.DependencyInjection;
using ThoughtEcho.Repository.Implementations;
using ThoughtEcho.Repository.Interfaces;

namespace ThoughtEcho.DependencyResolver
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
