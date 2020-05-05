using System;
using AutoMapper;
using BusinessServices;
using DataAccess;
using DataAccess.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Serilog;
using WebAPIService.Middleware;
using WebAPIService.Models;
using MessageBusServices;

namespace WebAPIService {
    public class Startup {
        private readonly string CorsPolicy = nameof (CorsPolicy);
        public IConfiguration Configuration { get; }

        public Startup (IConfiguration configuration) {
            Configuration = configuration;
            Log.Logger = new LoggerConfiguration ().ReadFrom.Configuration (Configuration).CreateLogger ();
        }

        private JsonSerializerSettings ConfigureJSON() {
            var result = new JsonSerializerSettings () {
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver (),
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore                
            };
            result.Converters.Add(new StringEnumConverter());
            return result;
        }

        public void ConfigureServices (IServiceCollection services) {
            JsonConvert.DefaultSettings = ConfigureJSON;
            services.Configure<ApplicationOptions> (Configuration.GetSection (nameof (ApplicationOptions)));
            
            var applicationOptions = new WebAPIService.Models.ApplicationOptions ();
            Configuration.GetSection (nameof (WebAPIService.Models.ApplicationOptions)).Bind (applicationOptions);            
            
            services.AddSwagger ();
            services.AddAuth (applicationOptions.IdentityServiceURI);
            services.AddSQL (Configuration.GetConnectionString ("DefaultConnection"));
            services.AddQueueService (applicationOptions.RabbitMQSeriveURI);
            services.AddBusinessServices();
            services.AddAutoMapper (typeof (Startup));
            
            
            services.AddControllers ()
                .AddNewtonsoftJson (options => {
                    options.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                    options.SerializerSettings.Formatting = Formatting.Indented;
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                });
            services.AddCors (options => {
                options.AddPolicy (nameof (CorsPolicy),
                    builder => builder.AllowAnyOrigin ()
                    .AllowAnyMethod ()
                    .AllowAnyHeader ()
                    .Build ());
            });
        }

        public void Configure (IApplicationBuilder app, IWebHostEnvironment env) {
            app.UseMiddleware<RequestResponseLoggingMiddleware> ();
            app.UseCors (nameof (CorsPolicy));
            
            app.UseCustomExceptionHandler();

            app.UseSwagger ();
            app.UseSwaggerUI (c => {
                c.SwaggerEndpoint ("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseRouting ();

            app.UseAuthentication ();
            app.UseAuthorization ();

            app.UseEndpoints (endpoints => {
                endpoints.MapControllers ();
            });
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory> ().CreateScope ()) {
                try {
                    var context = serviceScope.ServiceProvider.GetRequiredService<APIContext> ();                    
                    
                    context.Database.EnsureCreated();

                    if(!context.AllMigrationsApplied())
                    {
                        context.Database.Migrate();
                    }
                    
                } catch (Exception e) {
                    Log.Warning (e.Message);
                }
            }
        }
    }
}