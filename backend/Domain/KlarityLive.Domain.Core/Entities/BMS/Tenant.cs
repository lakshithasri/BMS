using KlarityLive.Domain.Core.Entities.Base;
using KlarityLive.Domain.Core.Enums;

namespace KlarityLive.Domain.Core.Entities.BMS
{
    public class Tenant : BaseEntity, ISoftDelete
    {
        public int BuildingId { get; set; }
        public string Name { get; set; }
        public string ContactPerson { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public TenantType TenantType { get; set; }
        public bool IsDeleted { get; set; }
        public List<TenantTenancy> Tenancies { get; set; }
    }
}
