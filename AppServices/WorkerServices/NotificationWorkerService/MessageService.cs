using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotificationWorkerService.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace NotificationWorkerService {
    public class MessageService {
        IModel Channel;
        RabbitMQSettings RabbitMQSettings;
        public MessageService (IModel channel, RabbitMQSettings rabbitMQSettings) {
            Channel = channel;
            RabbitMQSettings = rabbitMQSettings;
        }
        public void Dequeue (Action<string> messageHandler) {
            var consumer = new EventingBasicConsumer (Channel);
            consumer.Received += (sender, ea) => {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString (body.ToArray ());
                messageHandler (message);
            };
            foreach (var queue in RabbitMQSettings.Queues) {
                Channel.BasicConsume (queue: queue.QueueName,
                    autoAck: true,
                    consumer: consumer);
            }

        }
    }
}