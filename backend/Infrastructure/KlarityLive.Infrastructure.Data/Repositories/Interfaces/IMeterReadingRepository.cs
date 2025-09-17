using KlarityLive.Domain.Core.Entities.BMS;
using KlarityLive.Infrastructure.Data.Repositories.Base.Interfaces;

namespace KlarityLive.Infrastructure.Data.Repositories.Interfaces
{
    public interface IMeterReadingRepository 
    {
        Task<MeterReading> GetByIdAsync(int id, int meterId);
        Task<IEnumerable<MeterReading>> GetByMeterIdAsync(int meterId);
        Task<IEnumerable<MeterReading>> GetByMeterIdAndDateRangeAsync(int meterId, DateTime startDate, DateTime endDate);
        Task<IEnumerable<MeterReading>> GetLatestReadingsByMeterAsync(int meterId, int count = 10);
        Task<MeterReading> AddAsync(MeterReading meterReading);
        Task<MeterReading> UpdateAsync(MeterReading meterReading);
        Task DeleteAsync(int id, int meterId);
        Task<IEnumerable<MeterReading>> GetReadingsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<decimal> GetTotalConsumptionByMeterAsync(int meterId, DateTime startDate, DateTime endDate);
    }
}
