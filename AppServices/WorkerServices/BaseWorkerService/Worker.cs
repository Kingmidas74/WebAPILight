using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Serilog;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Extensions.Logging;
using Serilog.Extensions.Hosting;
using Serilog.Settings.Configuration;
using Newtonsoft.Json;
using BaseWorkerService.Models;
using BaseWorkerService.Enums;
using Microsoft.Extensions.Options;

namespace BaseWorkerService
{
    public class Worker : BackgroundService
    {
        public RabbitMQSettings RabbitMQSettings { get; }
        public Worker(IOptions<RabbitMQSettings> rabbitMQSettings)
        {
            RabbitMQSettings=rabbitMQSettings?.Value ?? throw new InvalidCastException($"{nameof(RabbitMQSettings)} was invalid!");
            RabbitMQSettings.Host=System.Environment.GetEnvironmentVariable(nameof(EnvironmentVariables.RMQ_HOST));
            RabbitMQSettings.Port=Int32.Parse(System.Environment.GetEnvironmentVariable(nameof(EnvironmentVariables.RMQ_PORT)));
            RabbitMQSettings.User=System.Environment.GetEnvironmentVariable(nameof(EnvironmentVariables.RMQ_USER));
            RabbitMQSettings.Password=System.Environment.GetEnvironmentVariable(nameof(EnvironmentVariables.RMQ_PASSWORD));
                    
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Log.Logger.Information("Worker running at: {time}", DateTimeOffset.Now);                
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
