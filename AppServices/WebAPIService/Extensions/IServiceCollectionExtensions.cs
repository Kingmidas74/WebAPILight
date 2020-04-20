using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

using Microsoft.OpenApi.Models;
using System;
using System.Reflection;
using System.IO;
using Newtonsoft.Json;
using WebAPIService.Middleware;
using WebAPIService.Models;
using Microsoft.Extensions.DependencyInjection;
using BusinessServices.Extensions;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Serilog;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WebAPIService
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddSQL(this IServiceCollection services, IConfiguration configuration)
        {
            DataAccess.DataAccessPolicy.ConnectionString = string.Format(configuration.GetConnectionString("DefaultConnection")
                                , System.Environment.GetEnvironmentVariable(nameof(EnvironmentVariables.API_DB_HOST))
                                , System.Environment.GetEnvironmentVariable(nameof(EnvironmentVariables.API_DB_PORT))
                                , System.Environment.GetEnvironmentVariable(nameof(EnvironmentVariables.API_DB_USER))
                                , System.Environment.GetEnvironmentVariable(nameof(EnvironmentVariables.API_DB_PASSWORD)));
            services.AddDbContextPool<DataAccess.DataBase>(options =>
            {
                options.UseNpgsql(DataAccess.DataAccessPolicy.ConnectionString);
                var extension = options.Options.FindExtension<CoreOptionsExtension>();
                if (extension != null)
                {
                    var loggerFactory = extension.ApplicationServiceProvider.GetService<ILoggerFactory>();
                    if (loggerFactory != null)
                    {
#if DEBUG
                        options.EnableSensitiveDataLogging().UseLoggerFactory(loggerFactory);
#else
                        options.UseLoggerFactory(loggerFactory);
#endif
                    }
                }
            });
            DataAccess.DataAccessPolicy.Init(services); 
            return services;
        }
        public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
        {
            var identityServerURI = string.Format(configuration.GetValue<string>("ApplicationOptions:IdentityServiceURI")
                    , System.Environment.GetEnvironmentVariable(nameof(EnvironmentVariables.PIS_HOST))
                    , System.Environment.GetEnvironmentVariable(nameof(EnvironmentVariables.PIS_PORT)));
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = identityServerURI;
                    options.RequireHttpsMetadata = false;

                    options.Audience = "phrygiawebapi";
                });
            /*services.AddAuthorization();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddIdentityServerAuthentication(
                        options =>
                        {
                            options.Authority = identityServerURI;
                            options.ApiName = "phrygiawebapi";                    
                            options.ApiSecret = "secret";
                            options.RequireHttpsMetadata = false;
                            options.EnableCaching = true;
                            options.CacheDuration = TimeSpan.FromMinutes(10);
                        })
                    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                        {
                            options.Authority = identityServerURI;
                            options.RequireHttpsMetadata = false;
                            options.Audience = "phrygiawebapi";
                        });*/
            return services;
        }
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Example API",
                    Description = "A simple example ASP.NET Core Web API",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Suleymanov Denis",
                        Email = string.Empty,
                        Url = new Uri("https://vk.com/iammidas"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under MIT",
                        Url = new Uri("https://example.com/license"),
                    }
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
                    In = ParameterLocation.Header, 
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey 
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                { 
                    new OpenApiSecurityScheme 
                    { 
                    Reference = new OpenApiReference 
                    { 
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer" 
                    } 
                    },
                    new string[] { } 
                    } 
                });


                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);                    
            });
            return services;
        }
    }
}