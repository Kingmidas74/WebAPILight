namespace NotificationWorkerService.Models {
    public partial class BindingSettings {
        public string QueueName { get; set; }
        public string ExchangeName { get; set; }
        public string RoutingKey { get; set; }
    }
}