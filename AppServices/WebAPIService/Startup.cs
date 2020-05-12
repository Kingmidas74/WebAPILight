using System;
using BusinessServices;
using DataAccess;
using DataAccess.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Serilog;
using WebAPIService.Middleware;
using WebAPIService.Models;
using MessageBusServices;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Prometheus;
using Masking.Serilog;

namespace WebAPIService
{   
    public class Startup {
        private readonly string CorsPolicy = nameof (CorsPolicy);
        public IConfiguration Configuration { get; }

        public Startup (IConfiguration configuration) {
            Configuration = configuration;          
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

            services.AddSingleton<MetricReporter>();

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

        public void Configure (IApplicationBuilder app, IApiVersionDescriptionProvider provider) {
            Log.Logger = new LoggerConfiguration ().ReadFrom.Configuration (Configuration)
                                .Destructure.ByMaskingProperties("Password", "Token")
                                .WriteTo.Seq(System.Environment.GetEnvironmentVariable(nameof(Models.EnvironmentVariables.SeqURL)))
                                .CreateLogger();
            
            app.UseMiddleware<RequestResponseLoggingMiddleware> ();
            app.UseMiddleware<ResponseMetricMiddleware>();
            app.UseMiddleware<CountRequestMiddleware>();
            
            app.UseStaticFiles();            

            app.UseCors (nameof (CorsPolicy));
            app.UseMetricServer(); 
            app.UseCustomExceptionHandler();
            app.UseHttpMetrics();
            app.UseDeveloperExceptionPage();  
            app.UseSwagger ();
            app.UseSwaggerUI (c => {
                foreach ( var description in provider.ApiVersionDescriptions )
                {
                    c.SwaggerEndpoint ($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                    c.RoutePrefix = "swagger";                    
                }
                
            });

            app.UseRouting ();

            app.UseAuthentication ();
            app.UseAuthorization ();

            app.UseEndpoints (endpoints => {
                endpoints.MapControllers ();
            });
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory> ().CreateScope ()) {
                var context = serviceScope.ServiceProvider.GetRequiredService<APIContext> ();                    
                    
                if(!context.AllMigrationsApplied())
                {
                    context.Database.Migrate();
                }                
            }
        }
    }
}