using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MasterServer.Application.Common.Interfaces
{

    public interface IWebsocketNofiticationService
    {
        Task SendMessageHandler(string packetId, object message, List<long> connectionIds);
    }

    public class WebsocketNofiticationService : IWebsocketNofiticationService
    {
        private readonly ILogger<WebsocketNofiticationService> _logger;
        private readonly IMessageQueue _queue;

        public WebsocketNofiticationService(IApplicationDbContext dbContext, ILogger<WebsocketNofiticationService> logger, IMessageQueue queue)
        {
            _logger = logger;
            _queue = queue;
        }

        public async Task SendMessageHandler(string packetId, object message, List<long> playerIds)
        {
            await Task.WhenAll(playerIds.Select(async x => await _queue.PublishAsync("player_" + x, JsonConvert.SerializeObject(new SsMessage
            {
                PacketId = packetId,
                Payload = JsonConvert.SerializeObject(message)
            }))));
        }
    }

    public class SsMessage
    {
        public string PacketId { get; set; }

        public string Payload { get; set; }

        public T As<T>()
        {
            return JsonConvert.DeserializeObject<T>(Payload);
        }    
    }
}
