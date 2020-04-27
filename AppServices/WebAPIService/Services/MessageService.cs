using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RabbitMQ.Client;

namespace WebAPIService.Services {
    public class MessageService {
        IModel Channel { get; }
        public MessageService (IModel rabbitMQChannel) {
            this.Channel = rabbitMQChannel;
        }
        public void Enqueue (string messageString, string routingKey) {
            var body = Encoding.UTF8.GetBytes ("message from webapi " + messageString);
            Channel.BasicPublish ("amq.direct", routingKey, null, body);
        }
    }
}