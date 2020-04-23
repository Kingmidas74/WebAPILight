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

namespace BaseWorkerService
{
    public class Worker : BackgroundService
    {
        public IConfiguration Configuration { get; }
        public Worker()
        {
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
