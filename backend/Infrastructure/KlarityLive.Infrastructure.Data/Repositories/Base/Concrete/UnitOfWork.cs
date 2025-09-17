using KlarityLive.Infrastructure.Data.DbContexts;
using KlarityLive.Infrastructure.Data.Repositories.Base.Interfaces;
using KlarityLive.Infrastructure.Data.Repositories.Concrete;
using KlarityLive.Infrastructure.Data.Repositories.Interfaces;

namespace KlarityLive.Infrastructure.Data.Repositories.Base.Concrete
{
    public class UnitOfWork(KlarityLiveDbContext context) : IUnitOfWork
    {
        private bool disposed = false;

        public IUserRepository UserRepository { get; private set; } = new UserRepository(context);
        public IRoleRepository RoleRepository { get; private set; } = new RoleRepository(context);
        public IBuildingRepository BuildingRepository { get; private set; } = new BuildingRepository(context);
        public IMeterRepository MeterRepository { get; private set; } = new MeterRepository(context);
        public IMeterReadingRepository MeterReadingRepository { get; private set; } = new MeterReadingRepository(context);
        public ITenancyRepository TenancyRepository { get; private set; } = new TenancyRepository(context);
        public ITenantRepository TenantRepository { get; private set; } = new TenantRepository(context);
        public ITenantTenancyRepository TenantTenancyRepository { get; private set; } = new TenantTenancyRepository(context);

        public async Task<int> SaveChangesAsync()
        {
            return await context.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            disposed = true;
        }

        /// <summary>
        /// Releases all resources used by the unit of work.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
