using DataAccess;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using Respawn;
using Respawn.Postgres;
using System.IO;
using System.Threading.Tasks;
using WebAPIService;

[SetUpFixture]
public class Testing
{   
    private static IConfigurationRoot _configuration;
    private static IServiceScopeFactory _scopeFactory;
    private static PostgresCheckpoint _checkpoint;

    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true)
            .AddEnvironmentVariables();

        _configuration = builder.Build();

        var startup = new Startup(_configuration);

        var services = new ServiceCollection();

        services.AddSingleton(Mock.Of<IWebHostEnvironment>(w =>
            w.EnvironmentName == "Development" &&
            w.ApplicationName == "WebAPIService"));

        services.AddLogging();

        startup.ConfigureServices(services);

        _scopeFactory = services.BuildServiceProvider().GetService<IServiceScopeFactory>();
        
        _checkpoint = new PostgresCheckpoint
        {
            TablesToIgnore = new [] { "__EFMigrationsHistory" },
            AutoCreateExtensions = true,

            SchemasToInclude = new[]
            {
                "public"
            }
        };

        EnsureDatabase();
    }

    private static void EnsureDatabase()
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetService<APIContext>();

        context.Database.Migrate();
    }

    public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    {
        using var scope = _scopeFactory.CreateScope();

        var mediator = scope.ServiceProvider.GetService<IMediator>();

        return await mediator.Send(request);
    }


    public static async Task ResetState()
    {
        await _checkpoint.Reset(_configuration.GetConnectionString("DefaultConnection"));        
    }

    public static async Task<TEntity> FindAsync<TEntity>(int id)
        where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetService<APIContext>();

        return await context.FindAsync<TEntity>(id);
    }

    public static async Task AddAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetService<APIContext>();

        context.Add(entity);

        await context.SaveChangesAsync();
    }

    [OneTimeTearDown]
    public void RunAfterAnyTests()
    {
    }
}