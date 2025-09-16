namespace KlarityLive.Core.Services.Interfaces
{
    public interface ICachingService
    {
        Task SetAsync<T>(string key, T value);
        Task<T> GetAsync<T>(string key);
        Task RemoveAsync(string key);
        Task SetAsync<T>(string key, T value, TimeSpan? expiration);
        Task<bool> IsExpiredAsync(string key);
        Task<bool> HasKeyAsync(string key);
        Task<int> FlushAsync(string pattern = "*");
    }
}
