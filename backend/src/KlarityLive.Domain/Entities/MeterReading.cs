using KlarityLive.Domain.Entities.Base;
using KlarityLive.Domain.Enums;

namespace KlarityLive.Domain.Entities
{
    public record MeterReading : BaseEntity
    {
        public int MeterId { get; set; }
        public int? TenancyId { get; set; }
        public DateTime ReadingDate { get; set; }
        public DateTime PeriodFromDate { get; set; }
        public DateTime PeriodToDate { get; set; }
        public int PeriodDays { get; set; }
        public decimal PreviousReading { get; set; }
        public decimal PresentReading { get; set; }
        public decimal? Advance { get; set; }
        public decimal Multiplier { get; set; } = 1.0m;
        public decimal Quantity { get; set; }
        public string MeasurementUnit { get; set; }
        public decimal? UtilityBillAmount { get; set; }
        public string UtilityBillUnit { get; set; }
        public ReadingSource Source { get; set; }
        public string Notes { get; set; }
        public virtual Meter Meter { get; set; }
        public virtual Tenancy Tenancy { get; set; }
    }
}
