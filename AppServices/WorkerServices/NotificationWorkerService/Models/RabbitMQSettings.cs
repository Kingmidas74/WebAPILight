namespace NotificationWorkerService.Models {
    public partial class RabbitMQSettings {
        public string RabbitMQSeriveURI { get; set; }
        public QueueSettings[] Queues { get; set; }
        public BindingSettings[] Bindings { get; set; }
    }
}