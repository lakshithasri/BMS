namespace KlarityLive.Core.Common.Security
{
    public class AppSecrets
    {
        public required string RedisConnectionString { get; set; }
        public required string DbConnectionString { get; set; }
        public required string SendGridApiKey { get; set; }
        public required string BlobStorageConnectionString { get; set; }
        public required string CosmosDbConnectionString { get; set; }
    }
}
