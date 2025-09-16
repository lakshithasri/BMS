namespace KlarityLive.Core.Security
{
    public class AppSecrets
    {
        public required virtual string DbConnectionString { get; set; }
        public required virtual string RedisConnectionString { get; set; }
        public required virtual string SendGridApiKey { get; init; }
        public required virtual string BlobStorageConnectionString { get; set; }
        public required virtual string CosmosAccountEndpoint { get; set; }
        public required virtual string CosmosAccountKey { get; set; }
        public required virtual string CosmosDatabaseName { get; set; }
    }
}
