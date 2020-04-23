using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Generic;
using QueueAdapter.Settings;
using QueueAdapter.Enums;

namespace QueueAdapter
{
    public class RetryQueue : MessageService
    {
        const string RetryExchangeArgument = @"x-dead-letter-exchange";
        const string RetryMessageTTLArgument = @"x-message-ttl";
        const string RetryNumberArgument = @"RetryNumber";

        private RetryQueueSettings _settings;
        private QueueSettings _queueSettings;

        public int RetryNumber { get; set; }

        private static Dictionary<string, object> GetRetryQueueArguments(string routeToExchangeName, int retryMessageTTL)
        {
            var args = new Dictionary<string, object>();
            args.Add(RetryExchangeArgument, routeToExchangeName);
            args.Add(RetryMessageTTLArgument, retryMessageTTL);
            return args;
        }

        public RetryQueue(RabbitMQSettings rabbitMQSettings,QueueSettings queueSettings, RetryQueueSettings settings) 
            : base(rabbitMQSettings,new QueueSettings
            {
                HostName = queueSettings.HostName,
                RetryCount = 0,
                RetryQueues = new RetryQueueSettings[0],
                QueueName = settings.RetryQueueName,
                ExchangeName = settings.RetryExchangeName,
                RoutingKey = queueSettings.RoutingKey
            }, GetRetryQueueArguments(queueSettings.ExchangeName, settings.RetryMessageTTL), QueueTypes.DIRECT)
        {
            _settings = settings;
            _queueSettings = queueSettings;
            RetryNumber = settings.RetryNumber;
        }

        public void Enqueue(BasicDeliverEventArgs args)
        {
            DoSomethingWithNewModel((model) =>
            {
                int retryNumber = GetRetryNumber(args);
                if (retryNumber < _queueSettings.RetryCount)
                {
                    var properties = model.CreateBasicProperties();
                    properties.Headers = new Dictionary<string, object>();
                    foreach (var header in args.BasicProperties.Headers)
                        properties.Headers[header.Key] = header.Value;

                    properties.Headers[RetryNumberArgument] = GetRetryNumber(args) + 1;

                    model.QueueBind(_settings.RetryQueueName, _settings.RetryExchangeName, _queueSettings.RoutingKey);
                    model.BasicPublish(_settings.RetryExchangeName, _queueSettings.RoutingKey, properties, args.Body);
                }
            });
        }

        public static int GetRetryNumber(BasicDeliverEventArgs args)
        {
            int retryNumber = 0;
            if (args.BasicProperties.Headers.ContainsKey(RetryNumberArgument))
            {
                retryNumber = (int)args.BasicProperties.Headers[RetryNumberArgument];
            }
            return retryNumber;
        }
    }
}