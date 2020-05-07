using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

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

        public static IServiceCollection AddSwagger (this IServiceCollection services) {

            services.AddApiVersioning(options => {
                options.ReportApiVersions=true;
                options.AssumeDefaultVersionWhenUnspecified=false;
                options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(0,1);                                
            });
            services.AddVersionedApiExplorer(options => {  
                options.GroupNameFormat ="VVV";  
                options.SubstituteApiVersionInUrl = true;  
            });
            services.AddSwaggerGen (c => {
                var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();  
  
                foreach (var description in provider.ApiVersionDescriptions)  
                {  
                    c.SwaggerDoc(description.GroupName, new OpenApiInfo()  
                    {  
                        Title = $"{typeof(Startup).Assembly.GetCustomAttribute<System.Reflection.AssemblyProductAttribute>().Product}",  
                        Version = description.ApiVersion.ToString(),  
                        Description = description.IsDeprecated ? $"{typeof(Startup).GetType().Assembly.GetCustomAttribute<AssemblyDescriptionAttribute>().Description} - DEPRECATED" : typeof(Startup).Assembly.GetCustomAttribute<AssemblyDescriptionAttribute>().Description,  
                        TermsOfService = new Uri ("https://example.com/terms"),
                        Contact = new OpenApiContact {
                            Name = "Suleymanov Denis",
                                Email = string.Empty,
                                Url = new Uri ("https://vk.com/iammidas"),
                        },
                        License = new OpenApiLicense {
                            Name = "Use under MIT",
                                Url = new Uri ("https://example.com/license"),
                        }                        
                    });  
                }  
                
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine (AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments (xmlPath);
            });
            return services;
        }
    }
}