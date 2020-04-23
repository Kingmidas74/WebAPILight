using System;

namespace QueueAdapter
{
    public class BaseMessage {
        public string MessageId { get; set; }
        public string Source { get; set; }
        public string Target { get; set; }
        public DateTime SentOn { get; set; }
        public int Priority { get; set; }
    }
}