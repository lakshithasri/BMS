using KlarityLive.Domain.Core.Entities.Base;
using KlarityLive.Domain.Core.Enums;

namespace KlarityLive.Domain.Core.Entities.BMS
{
    public class Tenancy : BaseEntity, ISoftDelete
    {
        public int BuildingId { get; set; }
        public string Name { get; set; }
        public string LeaseReference { get; set; }
        public DateTime LeaseStartDate { get; set; }
        public decimal RentAmount { get; set; }
        public TenancyStatus Status { get; set; }
        public string Category { get; set; }
        public bool IsDeleted { get; set; }
        public string Notes { get; set; }
    }
}
