using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using DDDSample1.Infrastructure.Shared;

namespace DDDSample1.Infrastructure.Provider 
{
    public class DbSqlServer : IDbProvider 
    {
        
        public void AddDbContext(IServiceCollection services, IConfiguration conf) 
        {
            services.AddDbContext<DDDSample1DbContext>(options =>
                options.UseSqlServer(conf.GetConnectionString("DefaultConnection"))
                .ReplaceService<IValueConverterSelector, StronglyEntityIdValueConverterSelector>());
        }

    }
}