using KlarityLive.Core.Common.DTOs.Building;
using KlarityLive.Core.Common.DTOs.Tenant;
using KlarityLive.Domain.Core.Entities.BMS;
using KlarityLive.Domain.Core.Enums;

namespace KlarityLive.Core.Common.DTOs.Tenancy
{
    public class TenancyDto
    {
        public int TenantId { get; set; }
        public string LeaseReference { get; set; }
        public DateTime LeaseStartDate { get; set; }
        public DateTime? LeaseEndDate { get; set; }
        public decimal? RentAmount { get; set; }
        public TenancyStatus Status { get; set; }
        public string Category { get; set; }
        public string Notes { get; set; }
        public virtual BuildingDto Building { get; set; }
        public virtual TenantDto Tenant { get; set; }
        public virtual ICollection<MeterReading> MeterReadings { get; set; }
    }
}
