using System;
using System.IO;
using System.Reflection;
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

                    options.Audience = "phrygiawebapi";
                });
            return services;
        }
        public static IServiceCollection AddSwagger (this IServiceCollection services) {
            services.AddSwaggerGen (c => {
                c.SwaggerDoc ("v1", new OpenApiInfo {
                    Version = "v1",
                        Title = "Example API",
                        Description = "A simple example ASP.NET Core Web API",
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