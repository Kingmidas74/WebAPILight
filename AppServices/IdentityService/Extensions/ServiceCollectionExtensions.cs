using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace IdentityService {
    public static class ServiceCollectionExtensions {
        public static IServiceCollection AddSQL (this IServiceCollection services, string connectionString) {            
            services.AddDbContextPool<AppDbContext> ((provider, options) => {

                options.UseNpgsql (
                    string.Format (connectionString, 
                                    System.Environment.GetEnvironmentVariable (nameof (EnvironmentVariables.PIS_DB_HOST)), 
                                    System.Environment.GetEnvironmentVariable (nameof (EnvironmentVariables.PIS_DB_PORT)), 
                                    System.Environment.GetEnvironmentVariable (nameof (EnvironmentVariables.PIS_DB_USER)), 
                                    System.Environment.GetEnvironmentVariable (nameof (EnvironmentVariables.PIS_DB_PASSWORD))), 
                    providerOptions => {
                                providerOptions.EnableRetryOnFailure (3);                                
                    });
                var extension = options.Options.FindExtension<CoreOptionsExtension> ();
                if (extension != null) {
                    var loggerFactory = extension.ApplicationServiceProvider.GetService<ILoggerFactory> ();
                    if (loggerFactory != null) {
#if DEBUG
                        options.EnableSensitiveDataLogging ().UseLoggerFactory (loggerFactory);
#else
                        options.UseLoggerFactory (loggerFactory);
#endif
                    }
                };
            });
            return services;
        }
    }
}