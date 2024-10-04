namespace MasterServer.Application.Common.Interfaces
{
    public interface ICacheHelperService
    {
        Task FlushGameConfig(string pattern);

        Task<T> TryToGetObjectAsync<T>(string key);

        Task<T> TryToGetObjectAsync<T>(string key, Func<Task<T>> callback);

        Task<T> TryToGetObjectAsync<T>(string key, Func<T> callback);

        Task<List<T>> TryToGetListAsync<T>(string key, Func<Task<List<T>>> callback);

        Task<List<T>> TryToGetListAsync<T>(string key, Func<List<T>> callback);


        Task<List<string>> TryToGetKeysByPattern(string key);

        Task RemoveAllKeyAsync(string[] keys);

        Task UpdateExpiryAsync(string key, TimeSpan timeSpan);

        Task AddExpiryAsync<T>(string key, T value, TimeSpan timeSpan);

        Task AddAlwayAsync<T>(string key, T value, TimeSpan timeSpan);

        Task AddAlwayAsync<T>(string key, T value, DateTimeOffset offset);

        Task AddAlwayAsync<T>(string key, T value);
    }
}
