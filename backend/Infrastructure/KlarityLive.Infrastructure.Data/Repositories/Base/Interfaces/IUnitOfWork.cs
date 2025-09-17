using KlarityLive.Infrastructure.Data.Repositories.Interfaces;

namespace KlarityLive.Infrastructure.Data.Repositories.Base.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IRoleRepository RoleRepository { get; }
        IBuildingRepository BuildingRepository { get; }
        IMeterRepository MeterRepository { get; }
        IMeterReadingRepository MeterReadingRepository { get; }
        ITenancyRepository TenancyRepository { get; }
        ITenantRepository TenantRepository { get; }
        ITenantTenancyRepository TenantTenancyRepository { get; }

        Task<int> SaveChangesAsync();
    }
}
