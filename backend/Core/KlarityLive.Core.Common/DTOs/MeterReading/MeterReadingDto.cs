using KlarityLive.Core.Common.DTOs.Tenancy;
using KlarityLive.Domain.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlarityLive.Core.Common.DTOs.MeterReading
{
    public class MeterReadingDto
    {
        public int Id { get; set; }
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
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

        // Navigation properties
        public virtual MeterDto Meter { get; set; }
        public virtual TenancyDto? Tenancy { get; set; }

        // Calculated properties
        public decimal ConsumptionDifference => PresentReading - PreviousReading;
        public decimal AdjustedConsumption => (ConsumptionDifference + (Advance ?? 0)) * Multiplier;
        public decimal DailyAverageConsumption => PeriodDays > 0 ? Quantity / PeriodDays : 0;
        public decimal? CostPerUnit => UtilityBillAmount.HasValue && Quantity > 0 ? UtilityBillAmount / Quantity : null;
    }

    public class CreateMeterReadingDto
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
        public string MeasurementUnit { get; set; } = string.Empty;
        public decimal? UtilityBillAmount { get; set; }
        public string UtilityBillUnit { get; set; } = string.Empty;
        public ReadingSource Source { get; set; } = ReadingSource.ExcelImport;
        public string Notes { get; set; } = string.Empty;
    }

    public class UpdateMeterReadingDto
    {
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
        public string Notes { get; set; } = string.Empty;
    }

    public class MeterReadingSummaryDto
    {
        public int MeterId { get; set; }
        public string MeterName { get; set; } = string.Empty;
        public int ReadingCount { get; set; }
        public DateTime? LastReadingDate { get; set; }
        public decimal? LastReading { get; set; }
        public decimal TotalConsumption { get; set; }
        public decimal AverageMonthlyConsumption { get; set; }
        public decimal? TotalCost { get; set; }
        public string MeasurementUnit { get; set; } = string.Empty;
    }
}
