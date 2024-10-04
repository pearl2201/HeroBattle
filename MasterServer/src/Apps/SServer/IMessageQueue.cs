namespace MasterServer.Application.Common.Interfaces
{
    public interface IChannel
    {
        void Dispose();

        void Subscribe(Action<string> cb);
    }
    public interface IMessageQueue
    {
        Task<IChannel> SubscribeChannelAsync(string channelName);

        Task PublishAsync(string channelName, string message);

        Task PublishAsync(string message);
    }
}
