using KlarityLive.Domain.Core.Entities.BMS;
using KlarityLive.Infrastructure.Data.DbContexts;
using KlarityLive.Infrastructure.Data.Repositories.Base.Concrete;
using KlarityLive.Infrastructure.Data.Repositories.Interfaces;

namespace KlarityLive.Infrastructure.Data.Repositories.Concrete
{
    public class TenantRepository(KlarityLiveDbContext context) : Repository<Tenant>(context), ITenantRepository
    {
    }
}
