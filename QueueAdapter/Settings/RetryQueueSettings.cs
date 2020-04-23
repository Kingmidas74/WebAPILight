namespace QueueAdapter.Settings
{
    public class RetryQueueSettings
    {
        public int RetryNumber { get; set; }
        public string RetryQueueName { get; set; }
        public string RetryExchangeName { get; set; }
        public int RetryMessageTTL { get; set; }
    }
}