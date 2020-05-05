using System.Text;
using MessageBusServices.Enums;
using RabbitMQ.Client;
using Domain.Extensions;
using System;

namespace MessageBusServices {
    public class MessageProducerService {
        IModel Channel { get; }
        public MessageProducerService (IModel rabbitMQChannel) {
            this.Channel = rabbitMQChannel;
        }
        public void Enqueue (string messageString, string routingKey) {
            var body = Encoding.UTF8.GetBytes ("message from webapi " + messageString);
            Channel.BasicPublish (AMQPExchanges.DIRECT.GetDescription(), routingKey, null, body);
        }

        public void Enqueue(string v, object p)
        {
            throw new NotImplementedException();
        }
    }
}