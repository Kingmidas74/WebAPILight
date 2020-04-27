using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;

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
using WebAPIService.Services;
using DataAccess;

namespace WebAPIService
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddSQL(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContextPool<APIContext<Guid>>((provider, options) =>
            {
                options.UseNpgsql(
                            string.Format(configuration.GetConnectionString("DefaultConnection")
                                , System.Environment.GetEnvironmentVariable(nameof(EnvironmentVariables.API_DB_HOST))
                                , System.Environment.GetEnvironmentVariable(nameof(EnvironmentVariables.API_DB_PORT))
                                , System.Environment.GetEnvironmentVariable(nameof(EnvironmentVariables.API_DB_USER))
                                , System.Environment.GetEnvironmentVariable(nameof(EnvironmentVariables.API_DB_PASSWORD)))
                            , providerOptions => providerOptions.EnableRetryOnFailure(3));
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
                };                
            });
            return services;
        }
        public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
        {
            Console.WriteLine(nameof(AddAuth));
            var applicationOptions = new ApplicationOptions();
            configuration.GetSection(nameof(ApplicationOptions)).Bind(applicationOptions);
            Console.WriteLine(applicationOptions.IdentityServiceURI);            
            var identityServerURI = string.Format(applicationOptions.IdentityServiceURI
                    , System.Environment.GetEnvironmentVariable(nameof(EnvironmentVariables.PIS_HOST))
                    , System.Environment.GetEnvironmentVariable(nameof(EnvironmentVariables.PIS_PORT)));
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = identityServerURI;
                    options.RequireHttpsMetadata = false;

                    options.Audience = "phrygiawebapi";
                });
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

        public static IServiceCollection AddQueueService(this IServiceCollection services, IConfiguration configuration)
        {
            var applicationOptions = new ApplicationOptions();
            configuration.GetSection(nameof(ApplicationOptions)).Bind(applicationOptions);
            Console.WriteLine(applicationOptions.RabbitMQSeriveURI);            
            var rabbitMQSeriveURI = string.Format(applicationOptions.RabbitMQSeriveURI
                    , System.Environment.GetEnvironmentVariable(nameof(EnvironmentVariables.RMQ_USER))
                    , System.Environment.GetEnvironmentVariable(nameof(EnvironmentVariables.RMQ_PASSWORD))
                    , System.Environment.GetEnvironmentVariable(nameof(EnvironmentVariables.RMQ_HOST))
                    , System.Environment.GetEnvironmentVariable(nameof(EnvironmentVariables.RMQ_PORT)));

            services.AddTransient<MessageService>(s=>
            {
                var factory = new ConnectionFactory {
                    Uri = new Uri(rabbitMQSeriveURI)
                };
                var connection = factory.CreateConnection();
                var channel = connection.CreateModel();
                return new MessageService(channel);
            });            
            return services;
        }

        public static void AddFactory<TService, TImplementation>(this IServiceCollection services) 
        where TService : class
        where TImplementation : class, TService
        {
            services.AddTransient<TService, TImplementation>();
            services.AddSingleton<Func<TService>>(x => () => x.GetService<TService>());
        }
    }
}