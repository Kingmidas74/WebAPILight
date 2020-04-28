using System;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Serilog;

namespace IdentityService {
    public class Startup {
        private readonly string CorsPolicy = nameof (CorsPolicy);
        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        public Startup (IConfiguration configuration, IWebHostEnvironment environment) {
            Configuration = configuration;
            Environment = environment;
            Log.Logger = new LoggerConfiguration ().ReadFrom.Configuration (Configuration).CreateLogger ();
        }

        public void ConfigureServices (IServiceCollection services) {
            services.Configure<ApplicationOptions> (Configuration.GetSection ("ApplicationOptions"));
            services.AddScoped<AppDbContext> ();
            services.AddTransient<IUserRepository, UserRepository> ();
            services.AddDbContextPool<AppDbContext> ((serviceProvider, optionsBuilder) => {
                optionsBuilder.UseNpgsql (
                    string.Format (Configuration.GetConnectionString ("DefaultConnection"), System.Environment.GetEnvironmentVariable (nameof (EnvironmentVariables.PIS_DB_HOST)), System.Environment.GetEnvironmentVariable (nameof (EnvironmentVariables.PIS_DB_PORT)), System.Environment.GetEnvironmentVariable (nameof (EnvironmentVariables.PIS_DB_USER)), System.Environment.GetEnvironmentVariable (nameof (EnvironmentVariables.PIS_DB_PASSWORD))), providerOptions => providerOptions.EnableRetryOnFailure (3));
            });
            services
                .AddIdentityServer (x => {
                    x.IssuerUri = System.Environment.GetEnvironmentVariable (nameof (EnvironmentVariables.PIS_DB_HOST));
                })
                .AddInMemoryIdentityResources (Configuration.GetSection ("IdentityService:IdentityResources").Get<IdentityResource[]> ())
                .AddInMemoryApiResources (Configuration.GetSection ("IdentityService:ApiResources").Get<ApiResource[]> ())
                .AddInMemoryClients (Configuration.GetSection ("IdentityService:Clients").Get<Client[]> ())
                .AddDeveloperSigningCredential ()
                .AddProfileService<ProfileService> ()
                .AddExtensionGrantValidator<PasswordValidator> ();
            services.AddControllers()
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

        public void Configure (IApplicationBuilder app) {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory> ().CreateScope ()) {
                
                try {
                    var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext> ();                    
                    
                    context.Database.EnsureCreated();

                    if(!context.AllMigrationsApplied())
                    {
                        context.Database.Migrate();
                    }
                    
                } catch (Exception e) {
                    Log.Warning (e.Message);
                }
            }
            if (Environment.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
            }

            app.UseCors (nameof (CorsPolicy));
            app.UseIdentityServer ();

            app.UseRouting ();
            app.UseEndpoints (endpoints => {
                endpoints.MapControllers ();
            });
        }
    }
}