using KlarityLive.Domain.Entities;
using KlarityLive.Infrastructure.Data.Context;
using KlarityLive.Infrastructure.Data.Repositories.Base.Interfaces;

namespace KlarityLive.Infrastructure.Data.Repositories.Base.Concrete
{
    public class BuildingRepository(KlarityLiveDbContext context) : Repository<Building>(context), IBuildingRepository
    {
    }
}
