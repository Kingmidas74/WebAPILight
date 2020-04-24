using System.Linq;

using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using RabbitMQ.Client;
using BaseWorkerService.Models;

namespace BaseWorkerService
{
    public interface IMessageService
    {
        bool Enqueue(string message);
    }

    public class MessageService : IMessageService
    {
        ConnectionFactory _factory;
        IConnection _conn;
        IModel _channel;
        RabbitMQSettings _rabbitMQSettings;
        public MessageService(RabbitMQSettings rabbitMQSettings)
        {
            _rabbitMQSettings = rabbitMQSettings;
            /*_factory = new ConnectionFactory() { HostName = rabbitMQSettings.Host, Port = rabbitMQSettings.Port };
            _factory.UserName = rabbitMQSettings.User;
            _factory.Password = rabbitMQSettings.Password;*/
            _factory = new ConnectionFactory {
                Uri = new Uri($"amqp://{rabbitMQSettings.User}:{rabbitMQSettings.Password}@{rabbitMQSettings.Host}:{rabbitMQSettings.Port}")
            };
            _conn = _factory.CreateConnection();            
            _channel = _conn.CreateModel();
            /*foreach(var exchange in rabbitMQSettings.Exchanges)
            {
                _channel.ExchangeDeclare(exchange.ExchangeName,exchange.ExchangeType.ToString(), exchange.Durable);
            }*/
            foreach(var queue in rabbitMQSettings.Queues)
            {
                _channel.QueueDeclare(queue: queue.QueueName,
                                        durable: queue.Durable,
                                        exclusive: false,
                                        autoDelete: false);                
            }
            foreach(var bind in rabbitMQSettings.Bindings)
            {
                _channel.QueueBind(bind.QueueName, bind.ExchangeName, bind.RoutingKey);    
            }
        }
        public bool Enqueue(string messageString)
        {            
            var body = Encoding.UTF8.GetBytes("server processed " + messageString);
            if (_conn == null || !_conn.IsOpen)
                _conn = _factory.CreateConnection();
            try
            {
                var rand = new Random();
                var routingKey = rand.Next(10)-1>4 ? "public":"private";
                _channel.BasicPublish("amq.direct", routingKey, null, body);
            }
            catch (Exception e)
            {
                //error = string.Format("Failed pushing data to queue name '{0}' on host '{1}' with user '{2}'. Error: {3}", "base.low.data", _factory.HostName, _factory.UserName);
                return false;
            }
            return true;
        }
    }
}