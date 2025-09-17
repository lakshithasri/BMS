using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using KlarityLive.Core.Common.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace KlarityLive.Core.Common.Services.Concrete
{
    public class KeyVaultService : IKeyVaultService
    {
        private readonly SecretClient _secretClient;
        private readonly string _encryptionKeyName;
        private readonly ILogger<KeyVaultService> _logger;
        private readonly IMemoryCache _cache;
        private readonly TimeSpan _cacheExpiration;

        public KeyVaultService(ILogger<KeyVaultService> logger, IMemoryCache cache)
        {
            var kvUrl = Environment.GetEnvironmentVariable("KeyVaultUrl")!;
            _encryptionKeyName = Environment.GetEnvironmentVariable("EncryptionSecretName")!;
            _logger = logger;
            _cache = cache;
            var credential = new DefaultAzureCredential();
            var vaultUri = new Uri(kvUrl);
            _secretClient = new SecretClient(new Uri(kvUrl), new DefaultAzureCredential());
            _cacheExpiration = TimeSpan.FromMinutes(30); // or get from configuration
        }

        public async Task<string> GetKeyAsync(string keyName)
        {
            _logger.LogInformation($"Reading key {keyName} from keyvault.");

            var cacheKey = $"keyvault_secret_{keyName}";

            if (_cache.TryGetValue(cacheKey, out string? cachedValue) && !string.IsNullOrEmpty(cachedValue))
            {
                _logger.LogDebug($"Retrieved key {keyName} from cache.");
                return cachedValue;
            }

            try
            {
                var key = await _secretClient.GetSecretAsync(keyName);
                var secretValue = key.Value.Value;

                // Cache the secret with expiration
                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = _cacheExpiration,
                    Priority = CacheItemPriority.High,
                    SlidingExpiration = TimeSpan.FromMinutes(10) // Reset expiration if accessed within 10 minutes
                };

                _cache.Set(cacheKey, secretValue, cacheOptions);

                _logger.LogDebug($"Cached key {keyName} for {_cacheExpiration.TotalMinutes} minutes.");

                return secretValue;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to retrieve key {keyName} from Key Vault.");

                // Fallback: try to get from cache even if expired (if available)
                if (_cache.TryGetValue(cacheKey, out string? fallbackValue) && !string.IsNullOrEmpty(fallbackValue))
                {
                    _logger.LogWarning($"Using expired cached value for key {keyName} due to Key Vault error.");
                    return fallbackValue;
                }

                throw;
            }
        }
    }
}
