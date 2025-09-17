using KlarityLive.Domain.Core.Entities.BMS;
using KlarityLive.Domain.Core.Exceptions;
using KlarityLive.Infrastructure.Data.DbContexts;
using KlarityLive.Infrastructure.Data.Repositories.Base.Concrete;
using KlarityLive.Infrastructure.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KlarityLive.Infrastructure.Data.Repositories.Concrete
{
    public class MeterReadingRepository : IMeterReadingRepository
    {
        private readonly KlarityLiveCosmosDbContext _context;

        public MeterReadingRepository(KlarityLiveCosmosDbContext context)
        {
            _context = context;
        }

        public async Task<MeterReading> GetByIdAsync(int id, int meterId)
        {
            try
            {
                var partitionKey = meterId.ToString();
                var entity = await _context.MeterReadings
                    .WithPartitionKey(partitionKey)
                    .FirstOrDefaultAsync(mr => mr.Id == id);

                return entity ?? throw new NotFoundException($"MeterReading with ID {id} was not found.");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"An error occurred while retrieving MeterReading with ID {id}.", ex);
            }
        }

        public async Task<IEnumerable<MeterReading>> GetByMeterIdAsync(int meterId)
        {
            try
            {
                var partitionKey = meterId.ToString();
                return await _context.MeterReadings
                    .WithPartitionKey(partitionKey)
                    .OrderByDescending(mr => mr.ReadingDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"An error occurred while retrieving readings for meter {meterId}.", ex);
            }
        }

        public async Task<IEnumerable<MeterReading>> GetByMeterIdAndDateRangeAsync(int meterId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var partitionKey = meterId.ToString();
                return await _context.MeterReadings
                    .WithPartitionKey(partitionKey)
                    .Where(mr => mr.ReadingDate >= startDate && mr.ReadingDate <= endDate)
                    .OrderByDescending(mr => mr.ReadingDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"An error occurred while retrieving readings for meter {meterId} in date range.", ex);
            }
        }

        public async Task<IEnumerable<MeterReading>> GetLatestReadingsByMeterAsync(int meterId, int count = 10)
        {
            try
            {
                var partitionKey = meterId.ToString();
                return await _context.MeterReadings
                    .WithPartitionKey(partitionKey)
                    .OrderByDescending(mr => mr.ReadingDate)
                    .Take(count)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"An error occurred while retrieving latest readings for meter {meterId}.", ex);
            }
        }

        public async Task<MeterReading> AddAsync(MeterReading meterReading)
        {
            try
            {
                meterReading.CreatedOn = DateTime.UtcNow;
                meterReading.UpdatedOn = DateTime.UtcNow;

                _context.MeterReadings.Add(meterReading);
                await _context.SaveChangesAsync();

                return meterReading;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while adding the meter reading.", ex);
            }
        }

        public async Task<MeterReading> UpdateAsync(MeterReading meterReading)
        {
            try
            {
                meterReading.UpdatedOn = DateTime.UtcNow;

                _context.MeterReadings.Update(meterReading);
                await _context.SaveChangesAsync();

                return meterReading;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while updating the meter reading.", ex);
            }
        }

        public async Task DeleteAsync(int id, int meterId)
        {
            try
            {
                var meterReading = await GetByIdAsync(id, meterId);

                _context.MeterReadings.Remove(meterReading);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while deleting the meter reading.", ex);
            }
        }

        public async Task<IEnumerable<MeterReading>> GetReadingsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                // Note: Cross-partition queries are expensive in Cosmos DB
                // Consider if you really need this or if you can partition differently
                return await _context.MeterReadings
                    .Where(mr => mr.ReadingDate >= startDate && mr.ReadingDate <= endDate)
                    .OrderByDescending(mr => mr.ReadingDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while retrieving readings by date range.", ex);
            }
        }

        public async Task<decimal> GetTotalConsumptionByMeterAsync(int meterId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var partitionKey = meterId.ToString();
                var readings = await _context.MeterReadings
                    .WithPartitionKey(partitionKey)
                    .Where(mr => mr.ReadingDate >= startDate && mr.ReadingDate <= endDate)
                    .ToListAsync();

                // Calculate total consumption based on your business logic
                // This is a simple example - you might need more complex calculation
                return readings.Sum(mr => mr.ReadingDate.Day); // Replace with actual consumption calculation
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"An error occurred while calculating consumption for meter {meterId}.", ex);
            }
        }
    }
}
