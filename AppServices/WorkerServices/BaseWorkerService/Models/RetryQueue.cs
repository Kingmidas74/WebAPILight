namespace BaseWorkerService.Models
{
    public partial class RetryQueue
    {
        public long RetryNumber { get; set; }
        public string RetryQueueName { get; set; }
        public string RetryExchangeName { get; set; }
        public long RetryMessageTtl { get; set; }
    }
}