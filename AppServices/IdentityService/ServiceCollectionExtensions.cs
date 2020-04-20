using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace IdentityService
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection UsePostgreSql(this IServiceCollection services, string connectionString)
        {
            services.AddEntityFrameworkSqlServer();
            services.AddDbContextPool<AppDbContext>((serviceProvider, optionsBuilder) =>
            {                
                optionsBuilder.UseNpgsql(connectionString);
                optionsBuilder.UseInternalServiceProvider(serviceProvider);                
            });
            NpgsqlConnection.GlobalTypeMapper.UseJsonNet();
            return services;
        }
    }
}