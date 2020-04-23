using System;
using System.Linq;
using RabbitMQ.Client.Events;
using QueueAdapter.Settings;

namespace QueueAdapter
{
    public class RetryManager : IDisposable
    {
        private QueueSettings _settings { get; set; }
        private RetryQueue[] _queues;

        public RetryManager(RabbitMQSettings rabbitMQSettings, QueueSettings settings)
        {
            _settings = settings;
            _queues = settings.RetryQueues?.Select(s => new RetryQueue(rabbitMQSettings, _settings, s)).OrderBy(s => s.RetryNumber).ToArray();
            _queues = _queues ?? new RetryQueue[0];
        }

        internal void Enqueue(BasicDeliverEventArgs args)
        {
            int retryNumber = RetryQueue.GetRetryNumber(args);

            if (_queues.Length > retryNumber)
                _queues[retryNumber].Enqueue(args);
        }

        public void Dispose()
        {
            _queues.ToList().ForEach(q => q.Dispose());
        }
    }
}