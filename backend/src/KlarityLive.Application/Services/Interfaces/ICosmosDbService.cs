using KlarityLive.Domain.Entities.Cosmos;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlarityLive.Application.Services.Interfaces
{
    public interface ICosmosDbService
    {
        Task CreateMeterReadingAsync(MeterReadingDocument reading);
        Task<IEnumerable<MeterReadingDocument>> GetMeterReadingsByMeterIdAsync(int meterId);
        Task<IEnumerable<T>> QueryItemsAsync<T>(QueryDefinition query, CancellationToken cancellationToken = default);

    }
}
