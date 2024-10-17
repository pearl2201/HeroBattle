using MasterServer.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterServer.Infrastructure.Services
{
    public class RabbitMqQueueService : IHostedService, IMessageQueue
    {

        private readonly IModel _channel;
        private readonly IServiceProvider _serviceProvider;
        public RabbitMqQueueService(IRabbitMqService rabbitMqService, IServiceProvider serviceProvider)
        {
            _channel = rabbitMqService.GetChannel();
            _serviceProvider = serviceProvider;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {



            var queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queue: queueName,
                              exchange: "broadcast",
                              routingKey: string.Empty);

            _channel.QueueBind(queue: queueName,
                  exchange: "broadcast",
                  routingKey: string.Empty);



            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += OnReceiveMessage;

            _channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);
        }

        public void OnReceiveMessage(object model, BasicDeliverEventArgs ea)
        {
            byte[] body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var ssMessage = JsonConvert.DeserializeObject<SsMessage>(message);
            if (ssMessage.PacketId == "connected")
            {

            }
            else if (ssMessage.PacketId == "disconnected")
            {

            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {

        }

        public Task<IChannel> SubscribeChannelAsync(string channelName)
        {
            throw new NotImplementedException();
        }

        public Task PublishAsync(string channelName, string message)
        {
            throw new NotImplementedException();
        }

        public Task PublishAsync(string message)
        {
            throw new NotImplementedException();
        }

        public Task PublishAsync(string packetId, object message, List<long> playerIds)
        {
            throw new NotImplementedException();
        }

        public Task BroadcastAsync(string packetId, object message)
        {
            throw new NotImplementedException();
        }
    }
}
