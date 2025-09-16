using KlarityLive.Application.Services.Interfaces;
using KlarityLive.Domain.Entities.Cosmos;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace KlarityLive.Application.Services.Concrete
{
    public class CosmosDbService : ICosmosDbService
    {
        private readonly ICosmosDbService _cosmosService;

        public CosmosDbService(ICosmosDbService cosmosService)
        {
            _cosmosService = cosmosService;
        }

        public async Task CreateMeterReadingAsync(MeterReadingDocument reading)
        {
            await _cosmosService.CreateItemAsync(reading, new PartitionKey(reading.MeterId));
        }

        public async Task<IEnumerable<MeterReadingDocument>> GetMeterReadingsByMeterIdAsync(int meterId)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.MeterId = @meterId ORDER BY c.ReadingDate DESC")
                .WithParameter("@meterId", meterId);

            var iterator = _cosmosService.GetItemQueryIterator<MeterReadingDocument>(query);
            var results = new List<MeterReadingDocument>();

            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                results.AddRange(response.ToList());
            }

            return results;
        }

        public async Task<IEnumerable<T>> QueryItemsAsync<T>(QueryDefinition query, CancellationToken cancellationToken = default)
        {
            var results = new List<T>();
            using var iterator = _cosmosService.GetItemQueryIterator<T>(query);
            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync(cancellationToken);
                results.AddRange(response);
            }
            return results;
        }
    }
}
