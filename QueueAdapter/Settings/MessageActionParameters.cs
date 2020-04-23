namespace QueueAdapter.Settings
{
    public class MessageActionParameters
    {
        public string Source { get; set; }
        public string ContractName { get; set; }
        public string MessageId { get; set; }
        public string QueueAlias { get; set; }
        public string Target { get; set; }
        public byte[] Message { get; set; }
    }
}