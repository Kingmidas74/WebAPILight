using System;
using AutoMapper;
using DataAccess;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Serilog;
using WebAPIService.MediatR;
using WebAPIService.Middleware;
using WebAPIService.Models;

namespace WebAPIService {
    public class Startup {
        private readonly string CorsPolicy = nameof (CorsPolicy);
        public IConfiguration Configuration { get; }

        public Startup (IConfiguration configuration) {
            Configuration = configuration;
            Log.Logger = new LoggerConfiguration ().ReadFrom.Configuration (Configuration).CreateLogger ();
        }

        public void ConfigureServices (IServiceCollection services) {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings () {
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver (),
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            services.Configure<ApplicationOptions> (Configuration.GetSection (nameof (ApplicationOptions)));

            services.AddSwagger ();
            services.AddAuth (Configuration);
            services.AddSQL (Configuration);
            services.AddQueueService (Configuration);
            services.AddServices();
            services.AddAutoMapper (typeof (Startup));
            services.AddMediatR(typeof(Startup));
            services.AddValidatorsFromAssembly(typeof(Startup).Assembly);            
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddControllers ()
                .AddNewtonsoftJson (options => {
                    options.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                    options.SerializerSettings.Formatting = Formatting.Indented;

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
                    var context = serviceScope.ServiceProvider.GetRequiredService<APIContext<Guid>> ();                    
                    
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