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
    public class Startup
    {
        private readonly string CorsPolicy=nameof(CorsPolicy);
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(Configuration).CreateLogger();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings()
            {
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            services.Configure<ApplicationOptions>(Configuration.GetSection(nameof(ApplicationOptions)));
            
            services.AddSwagger();
            services.AddAuth(Configuration);
            services.AddSQL(Configuration);

            services.AddControllers()
                    .AddNewtonsoftJson(options =>
                    {
                        options.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                        options.SerializerSettings.Formatting = Formatting.Indented;

                    });
            services.AddCors(options =>
            {
                options.AddPolicy(nameof(CorsPolicy),
                    builder => builder.AllowAnyOrigin()  
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                .Build());
            });            

            
            var provider = services.BuildServiceProvider();
            SCE.SC= provider;           
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {            
            
            app.UseMiddleware<RequestResponseLoggingMiddleware>();          
            app.UseCors(nameof(CorsPolicy));
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
