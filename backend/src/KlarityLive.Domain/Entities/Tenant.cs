using KlarityLive.Domain.Entities.Base;
using KlarityLive.Domain.Enums;

namespace KlarityLive.Domain.Entities
{
    public record Tenant : BaseEntity
    {
        public string Name { get; set; }
        public string ContactPerson { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public TenantType TenantType { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public virtual ICollection<Tenancy> Tenancies { get; set; }
    }
}
