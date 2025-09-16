using KlarityLive.Domain.Entities;
using KlarityLive.Domain.Entities.Cosmos;
using Microsoft.Azure.Cosmos;

namespace KlarityLive.Infrastructure.Data.Repositories.Base.Interfaces
{
    public interface IMeterReadingRepository : IRepository<MeterReading>
    {
        Task AddAsync(MeterReadingDocument reading);
        Task<IEnumerable<MeterReadingDocument>> GetByMeterIdAsync(int meterId);
        Task<IEnumerable<T>> QueryAsync<T>(QueryDefinition query, CancellationToken cancellationToken = default);
    }
}
