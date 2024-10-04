using MasterServer.Application.Common.Interfaces;
using Newtonsoft.Json;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace MasterServer.Infrastructure.Services
{
    public class RedisQueueService : IMessageQueue
    {
        private readonly IRedisDatabase _redisCacheClient;

        public RedisQueueService(IRedisDatabase redisCacheClient)
        {
            _redisCacheClient = redisCacheClient;
        }

        public async Task<IChannel> SubscribeChannelAsync(string channelName)
        {
            var channel = await _redisCacheClient.Database.Multiplexer.GetSubscriber().SubscribeAsync(channelName);
            return new RedisChannel(channelName, channel);
        }

        public Task PublishAsync(string channelName, string message)
        {
            return _redisCacheClient.Database.Multiplexer.GetSubscriber().PublishAsync(channelName, message);
        }

        public Task PublishAsync(string message)
        {
            return _redisCacheClient.Database.Multiplexer.GetSubscriber().PublishAsync("broadcast", message);
        }

        public Task PublishAsync(string packetId, object message, List<long> playerIds)
        {
            return Task.WhenAll(playerIds.Select(async x => await PublishAsync("player_" + x, JsonConvert.SerializeObject(new SsMessage
            {
                PacketId = packetId,
                Payload = JsonConvert.SerializeObject(message)
            }))));
        }

        public Task BroadcastAsync(string packetId, object message)
        {
            return PublishAsync("boardcast", JsonConvert.SerializeObject(new SsMessage
            {
                PacketId = packetId,
                Payload = JsonConvert.SerializeObject(message)
            }));
        }
    }

    public class RedisChannel : IChannel
    {
        private ChannelMessageQueue _channel;
        private string _channelName;

        public RedisChannel(string channelName, ChannelMessageQueue channel)
        {
            _channel = channel;
            _channelName = channelName;
        }

        public void Dispose()
        {
            _channel.Unsubscribe();
        }

        public void Subscribe(Action<string> cb)
        {
            _channel.OnMessage((cm) =>
            {
                cb.Invoke(cm.Message.ToString());
            });
        }
    }
}
