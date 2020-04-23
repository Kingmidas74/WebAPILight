using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Text;
using BaseWorkerService.Models;

namespace BaseWorkerService
{
    public class Program
    {
        public static int Main(string[] args)
        {
             try
            {
                Console.OutputEncoding = Encoding.UTF8;
                CreateHostBuilder(args).Build().Run();                
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Host terminated unexpectedly. {ex.Message}");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host
                .CreateDefaultBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env = hostingContext.HostingEnvironment;
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                          .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

                    config.AddEnvironmentVariables();
                    if (args != null)
                    {
                        config.AddCommandLine(args);
                    }
                })
                .ConfigureServices((hostContext, services) =>
                {   
                    IConfiguration configuration = hostContext.Configuration;                 
                    services.Configure<RabbitMQSettings>(configuration.GetSection(nameof(RabbitMQSettings)));                    
                    Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();                    
                    services.AddHostedService<Worker>();
                })
                .UseSerilog();
    }
}
