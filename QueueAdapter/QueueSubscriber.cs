using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using QueueAdapter.Settings;
using QueueAdapter.Enums;

namespace QueueAdapter
{
    public class QueueSubscriber : IDisposable
    {
        public event OnEvent OnHandler;

        private const int CheckStatusDelay = 1000;

        private bool _stopped = true;
        //private ITypeFactory _typeFactory;
        private bool _toStop { get; set; }
        private string _failedMessagesFolder { get; set; }
        private MessageService[] _queues { get; set; }
        private SubscriberSettings _settings { get; set; }

        private DateTime _lastCheckDate { get; set; }

        public bool Stopped
        {
            get
            {
                return _stopped;
            }
        }

        public string SubscriberName
        {
            get
            {
                return _settings.SubscriberName;
            }
        }

        public QueueSubscriber(RabbitMQSettings rabbitMQSettings, SubscriberSettings settings, /*ITypeFactory typeFactory = null,*/ string failedMessagesFolder = null)
        {
            _settings = settings;
            _toStop = false;
            //_typeFactory = typeFactory;
            _failedMessagesFolder = failedMessagesFolder;
            _queues = settings.Queues?.Select(s => new MessageService(rabbitMQSettings, s, queueType: QueueTypes.DIRECT)).ToArray();
        }

        public void Stop()
        {
            _toStop = true;
        }

        public void MessageProcessing(Action<BaseMessage> messageAction = null, Action<MessageActionParameters> rawAction = null, Action<string, Exception> logging = null)
        {
            _queues?.ToList().ForEach(q => q.RunConsumer(
                (MessageActionParameters parameters) =>
                {
                    Guid id = Guid.Empty;
                    BaseMessage instance = null;
                    try
                    {
                        if (messageAction != null)
                        {
                            instance = ToInstance(parameters.Message, parameters.ContractName);
                            var propId = instance.GetType().GetProperty("Id");
                            if (propId != null)
                            {
                                id = (Guid)propId.GetValue(instance);
                            }
                            messageAction.Invoke(instance);
                        }
                        else if (rawAction != null)
                        {
                            rawAction.Invoke(parameters);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                },
                logging: logging,
                onException: exception =>
                {
                    _toStop = true;
                    _stopped = true;
                    throw exception;
                }));

            while (!_toStop && (_queues?.All(q => q.IsOpen) ?? true))
                Thread.Sleep(CheckStatusDelay);

            _stopped = true;

            if ((!_queues?.All(q => q.IsOpen) ?? false))
                throw new QueueSubscriberException("Соединение прервано");
        }

        public BaseMessage ToInstance(byte[] data, string typeName)
        {
            /*if (_typeFactory == null)
                throw new QueueSubscriberException("Фабрика типов равна null");*/

            var stringBody = Encoding.UTF8.GetString(data);
            var instance = JsonConvert.DeserializeObject<BaseMessage>(stringBody);
            return instance as BaseMessage;
        }

        public void Publish(string source, string contractName, string messageId, string queueAlias, byte[] bytes, string target, IDictionary<string, object> arguments = null)
        {
            var qu = _queues?.FirstOrDefault(q => q.QueueAlias == queueAlias);

            OnHandler?.BeginInvoke($"queueAlias => {queueAlias}; ExchangeName => {qu.ExchangeName}; RoutingKey => {qu.RoutingKey}; QueueName => {qu.QueueName}", null, null);

            qu?.Publish(source, contractName, messageId, queueAlias, bytes, target: target, arguments: arguments);
        }

        public void Dispose()
        {
            _toStop = true;
            while (!_stopped)
                Thread.Sleep(CheckStatusDelay);

            _queues?.ToList().ForEach(q => q.Dispose());
        }
    }
}