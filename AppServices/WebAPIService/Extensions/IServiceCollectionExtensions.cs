using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace WebAPIService
{
    public static class IServiceCollectionExtensions {
        
        public static IServiceCollection AddAuth (this IServiceCollection services, string connectionString) {            
            var identityServerURI = string.Format (connectionString, 
                            System.Environment.GetEnvironmentVariable (nameof (WebAPIService.Models.EnvironmentVariables.PIS_HOST)), 
                            System.Environment.GetEnvironmentVariable (nameof (WebAPIService.Models.EnvironmentVariables.PIS_PORT)));
            services.AddAuthentication ("Bearer")
                .AddJwtBearer ("Bearer", options => {
                    options.Authority = identityServerURI;
                    options.RequireHttpsMetadata = false;

                    options.Audience = "phrygiawebapi offline_access";
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

                c.AddSecurityDefinition ("Bearer", new OpenApiSecurityScheme {
                    In = ParameterLocation.Header,
                        Description = "Please insert JWT with Bearer into field",
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement (new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
  
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine (AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments (xmlPath);
            });
            return services;
        }
    }
}