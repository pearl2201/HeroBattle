using MasterServer.Application.Common.Interfaces;
using RabbitMQ.Client;

namespace MasterServer.Infrastructure.Services
{
    public interface IRabbitMqService
    {
        IModel GetChannel();
    }
    public class RabbitMqService : IRabbitMqService
    {
        // localhost rabbitmq adress
        private readonly string _hostName = "localhost";

        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMqService()
        {
            ConnectionFactory connectionFactory = new ConnectionFactory()
            {
                //definitins for default rabbitmq connection user (guest).You can change your own server information.
                HostName = _hostName

            };

            _connection = connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(exchange: "broadcast", type: ExchangeType.Fanout);
            _channel.QueueDeclare(queue: "presence",
                                  durable: true,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);
        }

        public IModel GetChannel()
        {
            return _channel;
        }
    }
}
