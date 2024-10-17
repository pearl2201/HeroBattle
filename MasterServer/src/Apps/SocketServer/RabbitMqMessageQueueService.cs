using MasterServer.Application.Common.Interfaces;
using MasterServer.Application.Game.Socket.Commands;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace SServer
{
    public class RabbitMqMessageQueueService : IHostedService
    {
        private readonly ConnectionFactory _factory;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private SocketServer _socketServer;
        public RabbitMqMessageQueueService()
        {
            _factory = new ConnectionFactory { HostName = "localhost" };
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(exchange: "broadcast", type: ExchangeType.Fanout);

            // declare a server-named queue
            var queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queue: queueName,
                              exchange: "broadcast",
                              routingKey: string.Empty);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += OnReceiveMessage;
            _channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);

            _channel.QueueDeclare(queue: "presence",
              durable: true,
              exclusive: false,
              autoDelete: false,
              arguments: null);

        }

        public void RegisterSocketServer(SocketServer socketServer)
        {
            this._socketServer = socketServer;
        }

        public void OnReceiveMessage(object model, BasicDeliverEventArgs ea)
        {
            byte[] body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            if (ea.Exchange == "broadcast")
            {
                _socketServer.Boardcast(message);
            }
            else if (ea.RoutingKey.StartsWith("player_") && int.TryParse(ea.RoutingKey.Substring(6), out var playerId))
            {
                _socketServer.SendTo(playerId, message);
            }

        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {

        }

        public void QueueDeclareForPlayer(long playerId)
        {
            _channel.QueueDeclare(queue: "player_" + playerId,
              durable: true,
              exclusive: false,
              autoDelete: false,
              arguments: null);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _channel.Close();
            _connection.Close();

        }

        public void PublishPlayerConnect(long playerId, int connectionId, Guid sessionId)
        {

            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new SsMessage
            {
                PacketId = "connected",
                Payload = JsonConvert.SerializeObject(new SocketUserConnectedCommand()
                {
                    Id = playerId,
                    ConnectionId = connectionId,
                    SessionId = sessionId
                })
            }));

            var properties = _channel.CreateBasicProperties();

            properties.Persistent = true;

            _channel.BasicPublish(exchange: string.Empty,
                                 routingKey: "presence",
                                 basicProperties: properties,
                                 body: body);
        }

        public void PublishPlayerDisconnect(long playerId, int connectionId, Guid sessionId)
        {

            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new SsMessage
            {
                PacketId = "disconnected",
                Payload = JsonConvert.SerializeObject(new SocketUserConnectedCommand()
                {
                    Id = playerId,
                    ConnectionId = connectionId,
                    SessionId = sessionId
                })
            }));

            var properties = _channel.CreateBasicProperties();

            properties.Persistent = true;

            _channel.BasicPublish(exchange: string.Empty,
                                 routingKey: "presence",
                                 basicProperties: properties,
                                 body: body);
        }
    }
}
