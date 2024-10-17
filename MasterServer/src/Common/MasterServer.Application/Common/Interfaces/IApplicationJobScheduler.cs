using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Quartz;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace MasterServer.Application.Common.Interfaces
{
    public interface IWorkerJobNotifier
    {
        Task NotifyJobChange(Guid jobId, string jobKey, string jobGroup);

    }

    public interface IWorkerJobListener
    {
        Task SubscribeListenWorkerJobEvent();
    }

    public class WorkerJobNotifier : IWorkerJobNotifier, IWorkerJobListener, IDisposable
    {

        private readonly IServiceProvider _serviceProvider;
        private readonly IRedisDatabase _redisCacheClient;
        private ChannelMessageQueue _channel;


        public const string ChannelName = "worker-jobs";

        public WorkerJobNotifier(IServiceProvider serviceProvider, IRedisDatabase redisCacheClient)
        {

            _serviceProvider = serviceProvider;
            _redisCacheClient = redisCacheClient;

        }

        public async Task SubscribeListenWorkerJobEvent()
        {
            _channel = await _redisCacheClient.Database.Multiplexer.GetSubscriber().SubscribeAsync(ChannelName);
            _channel.OnMessage((msg) =>
            {
                OnReloadGameConfigEvent(msg);
            });
        }

        public async Task OnReloadGameConfigEvent(ChannelMessage msg)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
                var jobInfo = JsonConvert.DeserializeObject<JobInfo>(msg.Message);
                ISchedulerFactory schedulerFactory = scope.ServiceProvider.GetRequiredService<ISchedulerFactory>();
                var jobDataMap = new JobDataMap();
                jobDataMap["Id"] = jobInfo.JobId.ToString();
                var scheduler = await schedulerFactory.GetScheduler();
                await scheduler.TriggerJob(new JobKey(jobInfo.JobKey, jobInfo.JobGroup), jobDataMap);
            }
        }

        public void Dispose()
        {
            if (_channel != null)
            {
                _channel.Unsubscribe();
            }
        }

        private struct JobInfo
        {
            public Guid JobId { get; set; }

            public string JobKey { get; set; }

            public string JobGroup { get; set; }
        }

        public async Task NotifyJobChange(Guid jobId, string jobKey, string jobGroup)
        {
            await _redisCacheClient.Database.Multiplexer.GetSubscriber().PublishAsync(ChannelName, new RedisValue(JsonConvert.SerializeObject(new JobInfo
            {
                JobId = jobId,
                JobKey = jobKey,
                JobGroup = jobGroup
            })));
        }


    }
}
