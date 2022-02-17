using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DDDSample1.Infrastructure.Provider 
{
    public interface IDbProvider 
    {

        void AddDbContext(IServiceCollection services, IConfiguration conf);

    }
}