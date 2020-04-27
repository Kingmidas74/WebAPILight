using System;
using AutoMapper;
using BusinessServices.Extensions;
using DataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using Serilog;
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
            services.AddAutoMapper (typeof (Startup));
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
            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
            }

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
                var context = serviceScope.ServiceProvider.GetRequiredService<APIContext<Guid>> ();
                try {
                    context.Database.EnsureCreated ();
                    context.Database.Migrate ();
                } catch (Exception e) {
                    Log.Warning (e.Message);
                }
            }
        }
    }
}