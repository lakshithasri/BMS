using KlarityLive.Domain.Core.Entities.Base;

namespace KlarityLive.Domain.Core.Entities.BMS
{
    public class TenantTenancy : BaseEntity, ISoftDelete
    {
        public int TenantId { get; set; }
        public int TenancyId { get; set; }

        public bool IsDeleted { get; set; }

        public Tenant Tenant { get; set; }
        public Tenancy Tenancy { get; set; }
    }
}
