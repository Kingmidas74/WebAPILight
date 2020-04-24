namespace BaseWorkerService.Models
{
    public partial class RabbitMQSettings
    {
        public string WorkerName { get; set; }
        public string Host {get;set;}
        public int Port {get;set;}
        public string User {get;set;}
        public string Password {get;set;}
        public QueueSettings[] Queues { get; set; }
        public ExchangeSettings[] Exchanges { get; set; }
        public BindingSettings[] Bindings { get; set; }
    }
}