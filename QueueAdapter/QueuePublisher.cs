using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QueueAdapter.Settings;

namespace QueueAdapter
{
    public delegate void OnEvent(string text);
    public class QueuePublisher : IDisposable
    {
        public event OnEvent OnHandler;

        private QueueSubscriber[] _subscribers { get; set; }
        private PublisherSettings _settings { get; set; }
        private SubscribedContracts[] _subscribedContracts { get; set; }

        public QueuePublisher(RabbitMQSettings rabbitMQSettings, PublisherSettings settings, SubscribedContracts[] subscribedContracts)
        {
            _settings = settings;
            _subscribers = settings.Subscribers?.Select(s => new QueueSubscriber(rabbitMQSettings, s)).ToArray();
            _subscribedContracts = subscribedContracts;

            foreach(var t in _subscribers)
                t.OnHandler += T_OnHandler;
        }

        private void T_OnHandler(string text)
        {
            OnHandler?.BeginInvoke(text, null, null);
        }

        public void Publish(BaseMessage message)
        {
            Publish(message.Source, message.GetType().FullName, message.MessageId, MessageService.GetQueueAlias(message.Priority).ToString(), message, message.Target, true);
        }

        public void Publish(string source, string contractName, string messageId, string queueAlias, object data, string target, bool toDistributor, IDictionary<string, object> arguments = null)
        {
            byte[] bytes;
            if (data.GetType() == typeof(byte[]))
                bytes = (byte[])data;
            else
            {
                var body = JsonConvert.SerializeObject(data, Formatting.Indented);
                bytes = Encoding.UTF8.GetBytes(body);
            }

            if (string.IsNullOrWhiteSpace(target) || target == "all" || toDistributor)
            {
                foreach (var subscriber in _subscribers)
                {
                    var contracts = _subscribedContracts.Where(c => c.SubscriberName == subscriber.SubscriberName);
                    var all = contracts.SingleOrDefault()?.AllContracts ?? true;

                    if (subscriber.SubscriberName != source &&
                        (all || (contracts.FirstOrDefault()?.Contracts.Any(c => c == contractName) ?? false)))
                        subscriber.Publish(source, contractName, messageId, queueAlias, bytes, target, arguments);
                }
            }
            else
            {
                var subscriber = _subscribers.SingleOrDefault(s => s.SubscriberName == target);
                subscriber?.Publish(source, contractName, messageId, queueAlias, bytes, target, arguments);
            }
        }

        public void Dispose()
        {
            _subscribers.ToList().ForEach(q => q.Dispose());
        }
    }
}