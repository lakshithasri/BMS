using KlarityLive.Domain.Core.Entities.Base;
using KlarityLive.Domain.Core.Enums;

namespace KlarityLive.Domain.Core.Entities.BMS
{
    public class MeterReading : BaseEntity
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
        public decimal Multiplier { get; set; }
        public decimal Quantity { get; set; }
        public string MeasurementUnit { get; set; } = string.Empty;
        public decimal? UtilityBillAmount { get; set; }
        public string UtilityBillUnit { get; set; } = string.Empty;
        public ReadingSource Source { get; set; }
        public string Notes { get; set; } = string.Empty;

        // Navigation properties
        public virtual Meter Meter { get; set; }
        public virtual Tenancy? Tenancy { get; set; }

        // Calculated properties (read-only)
        public decimal ConsumptionDifference => PresentReading - PreviousReading;
        public decimal AdjustedConsumption => (ConsumptionDifference + (Advance ?? 0)) * Multiplier;
        public decimal DailyAverageConsumption => PeriodDays > 0 ? Quantity / PeriodDays : 0;
        public decimal? CostPerUnit => UtilityBillAmount.HasValue && Quantity > 0 ? UtilityBillAmount / Quantity : null;
    }
}
