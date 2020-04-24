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
using NotificationWorkerService.Models;
using NotificationWorkerService.Enums;
using Microsoft.Extensions.Options;

namespace NotificationWorkerService
{
    public class Worker : BackgroundService
    {
        private MessageService MessageService;
        public Worker(MessageService messageService)
        {
            MessageService=messageService;                    
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {     
            await Task.Run(()=>MessageService.Dequeue(message=>{
                Log.Logger.Information($"Worker running at: {DateTimeOffset.Now}. Message: {message}");     
            }));
        }
    }
}
