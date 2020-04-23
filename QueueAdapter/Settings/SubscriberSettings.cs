namespace QueueAdapter.Settings
{
    public class SubscriberSettings
    {
        public string SubscriberName { get; set; }
        public QueueSettings[] Queues { get; set; }
    }
}