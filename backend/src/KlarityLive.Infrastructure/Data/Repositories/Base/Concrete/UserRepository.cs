using KlarityLive.Domain.Entities;
using KlarityLive.Infrastructure.Data.Context;
using KlarityLive.Infrastructure.Data.Repositories.Base.Interfaces;

namespace KlarityLive.Infrastructure.Data.Repositories.Base.Concrete
{
    public class UserRepository(KlarityLiveDbContext context) : Repository<User>(context), IUserRepository
    {
    }
}
