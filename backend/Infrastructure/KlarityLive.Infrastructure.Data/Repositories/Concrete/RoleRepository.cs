using KlarityLive.Domain.Core.Entities.Amin;
using KlarityLive.Infrastructure.Data.DbContexts;
using KlarityLive.Infrastructure.Data.Repositories.Base.Concrete;
using KlarityLive.Infrastructure.Data.Repositories.Interfaces;

namespace KlarityLive.Infrastructure.Data.Repositories.Concrete
{
    public class RoleRepository(KlarityLiveDbContext context) : Repository<Role>(context), IRoleRepository
    {
    }
}
