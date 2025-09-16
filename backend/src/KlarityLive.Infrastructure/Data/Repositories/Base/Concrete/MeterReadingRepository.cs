using KlarityLive.Domain.Entities;
using KlarityLive.Infrastructure.Data.Context;
using KlarityLive.Infrastructure.Data.Repositories.Base.Interfaces;

namespace KlarityLive.Infrastructure.Data.Repositories.Base.Concrete
{
    public class MeterReadingRepository(KlarityLiveDbContext context) : Repository<MeterReading>(context), IMeterReadingRepository
    {
    }
}
