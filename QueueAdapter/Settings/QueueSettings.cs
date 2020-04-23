using System.Collections;
using System;
using System.Collections.Generic;

namespace QueueAdapter.Settings
{
    public class QueueSettings
    {
        public string QueueAlias { get; set; }
        public string HostName { get; set; }
        public string QueueName { get; set; }
        public string ExchangeName { get; set; }
        public string RoutingKey { get; set; }
        public ushort PrefetchCount { get; set; }
        public int RetryCount { get; set; }
        public RetryQueueSettings[] RetryQueues { get; set; }
        public IDictionary<string, object> Arguments { get; set; }
        
    }
}