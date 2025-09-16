using KlarityLive.Core.Services.Interfaces;
using StackExchange.Redis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace KlarityLive.Core.Services.Concrete
{
    public class CachingService : ICachingService
    {
        private readonly IConnectionMultiplexer _redisConnMultiplexer;
        private readonly IDatabase _cacheDb;

        public CachingService(IConnectionMultiplexer redisConnMultiplexer)
        {
            _redisConnMultiplexer = redisConnMultiplexer ?? throw new ArgumentException(nameof(redisConnMultiplexer));
            _cacheDb = _redisConnMultiplexer.GetDatabase() ?? throw new ArgumentException(nameof(_cacheDb));
        }

        /// <summary>
        /// Set cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">string key</param>
        /// <param name="value">T value</param>
        /// <returns>Task</returns>
        public async Task SetAsync<T>(string key, T value)
        {
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                MaxDepth = 64
            };

            string jsonString = JsonSerializer.Serialize(value, options);
            await _cacheDb.StringSetAsync(key, jsonString);
        }

        /// <summary>
        /// Get cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">string key</param>
        /// <returns>Task<T></returns>
        public async Task<T> GetAsync<T>(string key)
        {
            var cachedValue = await _cacheDb.StringGetAsync(key);

            if (cachedValue.HasValue)
            {
                return JsonSerializer.Deserialize<T>(cachedValue);
            }
            return default!; // Or throw an exception if no value found
        }

        /// <summary>
        /// Remove async
        /// </summary>
        /// <param name="key">string key</param>
        /// <returns>Task</returns>
        public async Task RemoveAsync(string key)
        {
            await _cacheDb.KeyDeleteAsync(key);
        }

        /// <summary>
        /// Update Async
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keyPrefix">string keyPrefix</param>
        /// <param name="value">dynamic value</param>
        /// <returns>Task</returns>
        public async Task UpdateAsync<T>(string keyPrefix, dynamic value)
        {
            var cachedUsers = await GetAsync<List<T>>($"{keyPrefix}");
            cachedUsers.Remove(value);

            await RemoveAsync($"{keyPrefix}");
            await SetAsync($"{keyPrefix}", cachedUsers);
        }

        public async Task<bool> IsExpiredAsync(string key)
        {
            var timeToLive = await _cacheDb.KeyTimeToLiveAsync(key);
            return !timeToLive.HasValue || timeToLive.Value < TimeSpan.Zero;
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration)
        {
            var serializedValue = JsonSerializer.Serialize(value);
            if (expiration.HasValue)
            {
                await _cacheDb.StringSetAsync(key, serializedValue, expiration);
            }
            else
            {
                await SetAsync(key, serializedValue);
            }
        }

        /// <summary>
        /// Clear cache entries matching a specific pattern
        /// </summary>
        /// <param name="pattern">Pattern to match keys (e.g. "user:*"). Use "*" to clear all keys.</param>
        /// <returns>Number of keys that were removed</returns>
        public async Task<int> FlushAsync(string pattern = "*")
        {
            int deletedCount = 0;
            try
            {
                // Get the first server endpoint
                var endpoint = _redisConnMultiplexer.GetEndPoints().FirstOrDefault();
                if (endpoint == null)
                {
                    throw new InvalidOperationException("No Redis server endpoints available.");
                }

                // Get server instance to perform key scanning
                var server = _redisConnMultiplexer.GetServer(endpoint);

                // Get all keys matching the pattern
                var keys = server.Keys(pattern: pattern).ToArray();

                // Delete each key
                foreach (var key in keys)
                {
                    if (await _cacheDb.KeyDeleteAsync(key))
                    {
                        deletedCount++;
                    }
                }

                return deletedCount;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error clearing Redis cache: {ex.Message}", ex);
            }
        }

        public async Task<bool> HasKeyAsync(string key)
        {
            return await _cacheDb.KeyExistsAsync(key);
        }
    }
}
