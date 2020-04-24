namespace BaseWorkerService.Models
{
    public partial class QueueSettings
    {
        public bool Durable { get; set; }
        public string QueueName { get; set; }
        public long PrefetchCount { get; set; }
    }
}