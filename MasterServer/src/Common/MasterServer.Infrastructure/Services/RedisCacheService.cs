using MasterServer.Application.Common.Interfaces;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace MasterServer.Infrastructure.Services
{

    public class RedisCacheService : ICacheHelperService, IMessageQueue
    {
        private readonly IRedisDatabase _redisCacheClient;

        public RedisCacheService(IRedisDatabase redisCacheClient)
        {
            _redisCacheClient = redisCacheClient;
        }
        public async Task FlushGameConfig(string pattern)
        {
            var keys = await TryToGetKeysByPattern(pattern);
            await RemoveAllKeyAsync(keys.ToArray());
        }

        public async Task<List<string>> TryToGetKeysByPattern(string key)
        {
            return (await _redisCacheClient.SearchKeysAsync(key)).ToList();
        }

        public async Task RemoveAllKeyAsync(string[] keys)
        {
            await _redisCacheClient.RemoveAllAsync(keys, StackExchange.Redis.CommandFlags.FireAndForget);

        }

        public async Task<List<T>> TryToGetListAsync<T>(string key, Func<Task<List<T>>> callback)
        {
            List<T> v;
            try
            {
                v = await _redisCacheClient.GetAsync<List<T>>(key, StackExchange.Redis.CommandFlags.PreferReplica);
                if (v == null || v.Count == 0)
                {
                    v = await callback();
                    await _redisCacheClient.AddAsync(key, v, TimeSpan.FromMinutes(15));
                }
            }
            catch (TimeoutException)
            {
                Serilog.Log.Error("TimeoutException on try to get {key} from redis", key);
                v = await callback();
            }
            catch (Exception e)
            {
                Serilog.Log.Error("{exception} on try to get {key} from redis", e, key);
                v = await callback();
            }
            return v;
        }

        public async Task<List<T>> TryToGetListAsync<T>(string key, Func<List<T>> callback)
        {
            List<T> v;
            try
            {
                v = await _redisCacheClient.GetAsync<List<T>>(key, StackExchange.Redis.CommandFlags.PreferReplica);
                if (v == null || v.Count == 0)
                {
                    v = callback();
                    await _redisCacheClient.AddAsync(key, v, TimeSpan.FromMinutes(15));
                }
            }
            catch (TimeoutException)
            {
                Serilog.Log.Error("TimeoutException on try to get {key} from redis", key);
                v = callback();
            }
            catch (Exception e)
            {
                Serilog.Log.Error("{exception} on try to get {key} from redis", e, key);
                v = callback();
            }
            return v;
        }

        public async Task<T> TryToGetObjectAsync<T>(string key, Func<Task<T>> callback)
        {
            T v;
            try
            {
                v = await _redisCacheClient.GetAsync<T>(key, StackExchange.Redis.CommandFlags.PreferReplica);
                if (v == null)
                {
                    v = await callback();
                    await _redisCacheClient.AddAsync(key, v, TimeSpan.FromMinutes(15));
                }
            }
            catch (TimeoutException)
            {
                Serilog.Log.Error("TimeoutException on try to get {key} from redis", key);
                v = await callback();
            }
            catch (Exception e)
            {
                Serilog.Log.Error("{exception} on try to get {key} from redis", e, key);
                v = await callback();
            }
            return v;
        }

        public async Task<T> TryToGetObjectAsync<T>(string key, Func<T> callback)
        {
            T v;
            try
            {
                v = await _redisCacheClient.GetAsync<T>(key, StackExchange.Redis.CommandFlags.PreferReplica);
                if (v == null)
                {
                    v = callback();
                    await _redisCacheClient.AddAsync(key, v, TimeSpan.FromMinutes(15));
                }
            }
            catch (TimeoutException)
            {
                Serilog.Log.Error("TimeoutException on try to get {key} from redis", key);
                v = callback();
            }
            catch (Exception e)
            {
                Serilog.Log.Error("{exception} on try to get {key} from redis", e, key);
                v = callback();
            }
            return v;
        }

        public async Task<T> TryToGetObjectAsync<T>(string key, Func<Task<T>> callback, TimeSpan expiredIn, HashSet<string> tags = null)
        {
            T v;
            try
            {
                v = await _redisCacheClient.GetAsync<T>(key, StackExchange.Redis.CommandFlags.PreferReplica);
                if (v == null)
                {
                    v = await callback();
                    if (tags == null)
                    {
                        await _redisCacheClient.AddAsync(key, v, expiredIn);
                    }
                    else
                    {
                        await _redisCacheClient.AddAsync(key, v, expiredIn, tags: tags);
                    }

                }
            }
            catch (TimeoutException)
            {
                Serilog.Log.Error("TimeoutException on try to get {key} from redis", key, expiredIn);
                v = await callback();
            }
            catch (Exception e)
            {
                Serilog.Log.Error("{exception} on try to get {key} from redis", e, key);
                v = await callback();
            }
            return v;
        }

        public Task UpdateExpiryAsync(string key, TimeSpan timeSpan)
        {
            return _redisCacheClient.UpdateExpiryAsync(key, timeSpan);
        }

        public Task AddExpiryAsync<T>(string key, T value, TimeSpan timeSpan)
        {
            return _redisCacheClient.AddAsync(key, value, timeSpan, StackExchange.Redis.When.NotExists);
        }

        public Task AddAlwayAsync<T>(string key, T value, TimeSpan timeSpan)
        {
            return _redisCacheClient.AddAsync(key, value, timeSpan, StackExchange.Redis.When.Always);
        }

        public Task AddAlwayAsync<T>(string key, T value)
        {
            return _redisCacheClient.AddAsync(key, value, StackExchange.Redis.When.Always);
        }


        public async Task<T> TryToGetObjectAsync<T>(string key)
        {
            T v = default(T);
            try
            {
                v = await _redisCacheClient.GetAsync<T>(key, StackExchange.Redis.CommandFlags.PreferReplica);
            }
            catch (TimeoutException)
            {
                Serilog.Log.Error("TimeoutException on try to get {key} from redis", key);

            }
            catch (Exception e)
            {
                Serilog.Log.Error("{exception} on try to get {key} from redis", e, key);

            }
            return v;
        }

        public async Task RemoveAllTagsAsync(HashSet<string> tags)
        {
            foreach (var tag in tags)
            {
                await _redisCacheClient.RemoveByTagAsync(tag);
            }

        }

        public async Task RemoveTagAsync(string tag)
        {
            await _redisCacheClient.RemoveByTagAsync(tag);
        }

        public Task AddAlwayAsync<T>(string key, T value, DateTimeOffset offset)
        {
            return _redisCacheClient.AddAsync(key, value, offset, StackExchange.Redis.When.Always);
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
