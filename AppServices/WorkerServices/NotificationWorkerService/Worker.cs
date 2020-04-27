using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NotificationWorkerService.Enums;
using NotificationWorkerService.Models;
using Serilog;
using Serilog.Extensions.Hosting;
using Serilog.Extensions.Logging;
using Serilog.Settings.Configuration;

namespace NotificationWorkerService {
    public class Worker : BackgroundService {
        private MessageService MessageService;
        public Worker (MessageService messageService) {
            MessageService = messageService;
        }

        protected override async Task ExecuteAsync (CancellationToken stoppingToken) {
            await Task.Run (() => MessageService.Dequeue (message => {
                Log.Logger.Information ($"Worker running at: {DateTimeOffset.Now}. Message: {message}");
            }));
        }
    }
}