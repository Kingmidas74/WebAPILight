using System;
using QueueAdapter.Settings;
using System.Text;
using RabbitMQ.Client;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Threading.Tasks;
using QueueAdapter.Enums;
using RabbitMQ.Client.Events;

namespace QueueAdapter
{
    public class MessageService : IDisposable
    {
        private IConnectionFactory _factory;
        IConnection _connection;
        IModel _model;

        private RetryManager _retryManager;

        private QueueSettings _settings { get; set; }

        private string _failedMessagesFolder { get; set; }

        public bool IsOpen
        {
            get
            {
                return _model != null ? _model.IsOpen : false;
            }
        }

        public string QueueAlias
        {
            get
            {
                return _settings.QueueAlias;
            }
        }

        public string ExchangeName
        {
            get
            {
                return _settings.ExchangeName;
            }
        }

        public string RoutingKey
        {
            get
            {
                return _settings.RoutingKey;
            }
        }

        public string QueueName
        {
            get
            {
                return _settings.QueueName;
            }
        }

        public ushort PrefetchCount
        {
            get
            {
                return _settings.PrefetchCount;
            }
        }

        private void InitializeExchangeAndQueue(IDictionary<string, object> arguments, QueueTypes queueType)
        {
            try
            {
                DoSomethingWithModel((model) => 
                    model.ExchangeDeclarePassive(_settings.ExchangeName));
            }
            catch
            {
                DoSomethingWithModel((model) =>
                    model.ExchangeDeclare(_settings.ExchangeName, queueType.ToString().ToLower(), durable: true, autoDelete: false));
            }

            try
            {
                DoSomethingWithModel((model) =>
                    model.QueueDeclarePassive(_settings.QueueName));
            }
            catch
            {
                DoSomethingWithModel((model) =>
                    model.QueueDeclare(_settings.QueueName, durable: true, exclusive: false, autoDelete: false, arguments: arguments));
            }
        }

        public MessageService(RabbitMQSettings rabbitMQSettings, QueueSettings queueSettings, IDictionary<string, object> args = null, QueueTypes queueType = QueueTypes.DIRECT, string failedMessagesFolder = null)
        {
            _settings = queueSettings;
            _failedMessagesFolder = failedMessagesFolder;

            _factory = new ConnectionFactory { HostName = rabbitMQSettings.Host, Port=rabbitMQSettings.Port, UserName=rabbitMQSettings.User, Password=rabbitMQSettings.Password, AutomaticRecoveryEnabled=true };
            _connection = _factory.CreateConnection();
            _model = _connection.CreateModel();

            _retryManager = new RetryManager(rabbitMQSettings,queueSettings);

            InitializeExchangeAndQueue(args?? queueSettings.Arguments, queueType);
        }

        public virtual void Dispose()
        {
            _retryManager.Dispose();
            if (_model != null && _model.IsOpen)
            {
                _model.Close();
                _model.Dispose();
            }
            if (_connection != null && _connection.IsOpen)
            {
                _connection.Close();
                _connection.Dispose();
            }
        }

        private IConnection GetConnection()
        {
            return _factory.CreateConnection();
        }

        public void DoSomethingWithModel(Action<IModel> action)
        {
            if (_connection == null || !_connection.IsOpen)
            {
                _connection = _factory.CreateConnection();
                _model = _connection.CreateModel();
            }

            if (_model.IsClosed)
                _model = _connection.CreateModel();

            action.Invoke(_model);
        }

        public void DoSomethingWithNewModel(Action<IModel> action)
        {
            if (_connection == null || !_connection.IsOpen)
            {
                _connection = _factory.CreateConnection();
                _model = _connection.CreateModel();
            }

            using (var model = _connection.CreateModel())
            {
                action.Invoke(model);
            }
        }

        internal void RunConsumer(Action<MessageActionParameters> consumerAction, Action<string, Exception> logging, Action<Exception> onException)
        {
            var consumer = new EventingBasicConsumer(_model);
            consumer.Received += new EventHandler<BasicDeliverEventArgs>((object sender, BasicDeliverEventArgs args) =>
            {
                Task.Run(() => {
                    MessageActionParameters parameters = null;
                    try
                    {
                        parameters = ResolveMessageInfo(args);
                        consumerAction.Invoke(parameters);
                        _model.BasicAck(args.DeliveryTag, false);
                    }
                    catch (Exception exception)
                    {
                        if (_model.IsOpen)
                        {
                            logging?.Invoke(parameters?.MessageId,
                                new QueueSubscriberException(string.Join(";", parameters?.Source, parameters?.ContractName, parameters?.MessageId), exception));


                            try { _retryManager.Enqueue(args); } catch { }
                            try { _model.BasicAck(args.DeliveryTag, false); } catch { }
                        }

                        if (_model.IsClosed)
                        {
                            onException?.Invoke(new QueueSubscriberException("Соединение прервано", exception));
                        }
                    }
                });
            });
            _model.BasicConsume(consumer, QueueName);
            _model.BasicQos(0, PrefetchCount, true);
            _model.QueueBind(QueueName, ExchangeName, RoutingKey);
        }

        private MessageActionParameters ResolveMessageInfo(BasicDeliverEventArgs args)
        {
            var parameters = new MessageActionParameters();
            parameters.Message = args.Body.ToArray();

            if (args.BasicProperties.Headers != null)
            {
                if (args.BasicProperties.Headers.ContainsKey("Source"))
                    parameters.Source = Encoding.UTF8.GetString(args.BasicProperties.Headers["Source"] as byte[]);
                else parameters.Source = null;
                if (args.BasicProperties.Headers.ContainsKey("ContractName"))
                    parameters.ContractName = Encoding.UTF8.GetString(args.BasicProperties.Headers["ContractName"] as byte[]);
                else throw new ArgumentNullException("Отсутствует имя контракта в заголовке сообщения");

                if (args.BasicProperties.Headers.ContainsKey("MessageId"))
                    parameters.MessageId = Encoding.UTF8.GetString(args.BasicProperties.Headers["MessageId"] as byte[]);
                else parameters.MessageId = null;

                if (args.BasicProperties.Headers.ContainsKey("Priority"))
                    parameters.QueueAlias = Encoding.UTF8.GetString(args.BasicProperties.Headers["Priority"] as byte[]);
                else parameters.QueueAlias = QueuePriorities.NORMAL.ToString();

                if (args.BasicProperties.Headers.ContainsKey("Target"))
                    parameters.Target = Encoding.UTF8.GetString(args.BasicProperties.Headers["Target"] as byte[]);
                else parameters.Target = null;
            }
            else throw new QueueSubscriberException("Отсутствует заголовок сообщения");

            return parameters;
        }

        public void Publish(string source, string contractName, string messageId, string queueAlias, byte[] bytes, string target, IDictionary<string, object> arguments = null)
        {
            DoSomethingWithNewModel(model =>
            {
                model.QueueBind(QueueName, ExchangeName, RoutingKey, arguments);
                IBasicProperties properties = model.CreateBasicProperties();
                properties.DeliveryMode = 2;
                properties.Headers = new Dictionary<string, object>();
                properties.Headers["Source"] = Encoding.UTF8.GetBytes(source ?? string.Empty);
                properties.Headers["ContractName"] = Encoding.UTF8.GetBytes(contractName ?? string.Empty);
                properties.Headers["MessageId"] = Encoding.UTF8.GetBytes(messageId ?? string.Empty);
                properties.Headers["Priority"] = Encoding.UTF8.GetBytes(queueAlias ?? string.Empty);
                if (!string.IsNullOrWhiteSpace(target))
                    properties.Headers["Target"] = Encoding.UTF8.GetBytes(target);
                model.BasicPublish(ExchangeName, RoutingKey, properties, bytes);
            });
        }

        public static QueuePriorities GetQueueAlias(int priority)
        {
            switch (priority)
            {
                case 200:
                    return QueuePriorities.SERVICE;
                case 0:
                    return QueuePriorities.LOW;
                case 1:
                    return QueuePriorities.NORMAL;
                case 2:
                    return QueuePriorities.HIHG;
                default:
                    return QueuePriorities.NORMAL;
            }
        }

        public static int GetPriorityByAlias(string queueAlias)
        {
            switch (Enum.Parse(typeof(QueuePriorities),queueAlias))
            {
                case QueuePriorities.LOW: return 0;
                case QueuePriorities.NORMAL: return 1;
                case QueuePriorities.HIHG: return 2;
                case QueuePriorities.SERVICE: return 200;
                default: return 0;
            }
        }

        public void DeleteQueueAndExchange()
        {
            DoSomethingWithModel(model => 
            {
                model.QueueDelete(_settings.QueueName, false, false);
                model.ExchangeDelete(_settings.ExchangeName);
            });
        }
    }
}
