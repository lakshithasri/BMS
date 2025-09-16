using KlarityLive.Domain.Entities;
using KlarityLive.Infrastructure.Data.Context;
using KlarityLive.Infrastructure.Data.Repositories.Base.Interfaces;

namespace KlarityLive.Infrastructure.Data.Repositories.Base.Concrete
{
    public class TenantRepository(KlarityLiveDbContext context) : Repository<Tenant>(context), ITenantRepository
    {
    }
}
