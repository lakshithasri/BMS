using KlarityLive.Core.Common.DTOs.Building;
using KlarityLive.Domain.Core.Enums;

namespace KlarityLive.Core.Common.DTOs.Tenant
{
    public class TenantDto
    {
        public int Id { get; set; }
        public int BuildingId { get; set; }
        public string Name { get; set; }
        public string ContactPerson { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public TenantType TenantType { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public BuildingDto Building { get; set; }
    }
}
