using KlarityLive.Domain.Entities.Base;
using KlarityLive.Domain.Enums;

namespace KlarityLive.Domain.Entities
{
    public record Tenancy : BaseEntity
    {
        public int PropertyId { get; set; }
        public int TenantId { get; set; }
        public string LeaseReference { get; set; }
        public DateTime LeaseStartDate { get; set; }
        public DateTime? LeaseEndDate { get; set; }
        public decimal? RentAmount { get; set; }
        public TenancyStatus Status { get; set; }
        public string Category { get; set; }
        public string Notes { get; set; }
        public virtual Building Building { get; set; }
        public virtual Tenant Tenant { get; set; }
        public virtual ICollection<MeterReading> MeterReadings { get; set; }
    }
}
