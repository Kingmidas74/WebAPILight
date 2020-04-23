namespace BaseWorkerService.Models
{
    public partial class QueueSettings
    {
        public string QueuePriority { get; set; }
        public string QueueName { get; set; }
        public string ExchangeName { get; set; }
        public string RoutingKey { get; set; }
        public long PrefetchCount { get; set; }
        public long RetryCount { get; set; }
        public RetryQueue[] RetryQueues { get; set; }
        public string HostName { get; set; }
        public string QueueAlias { get; set; }
    }
}