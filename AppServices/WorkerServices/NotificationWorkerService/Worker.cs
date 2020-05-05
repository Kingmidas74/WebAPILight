using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace NotificationWorkerService
{
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