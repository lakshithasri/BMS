using KlarityLive.Core.Common.DTOs.Building;
using KlarityLive.Domain.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlarityLive.Core.Common.DTOs.MeterDto
{
    public class MeterDto
    {
        public int Id { get; set; }
        public int BuildingId { get; set; }
        public string MeterName { get; set; } = string.Empty;
        public string MeterNumber { get; set; } = string.Empty;
        public string Register { get; set; } = string.Empty;
        public MeterType MeterType { get; set; }
        public string MeasurementUnit { get; set; } = string.Empty;
        public decimal Multiplier { get; set; }
        public bool IsActive { get; set; }
        public DateTime InstallationDate { get; set; }
        public DateTime? RemovalDate { get; set; }
        public string Notes { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

        // Navigation properties
        public virtual BuildingDto? Building { get; set; }

        // Summary properties
        public int? TotalReadingsCount { get; set; }
        public DateTime? LastReadingDate { get; set; }
        public decimal? LastReadingValue { get; set; }
    }

    public class CreateMeterDto
    {
        public int BuildingId { get; set; }
        public string MeterName { get; set; } = string.Empty;
        public string MeterNumber { get; set; } = string.Empty;
        public string Register { get; set; } = string.Empty;
        public MeterType MeterType { get; set; }
        public string MeasurementUnit { get; set; } = string.Empty;
        public decimal Multiplier { get; set; } = 1.0m;
        public bool IsActive { get; set; } = true;
        public DateTime InstallationDate { get; set; }
        public DateTime? RemovalDate { get; set; }
        public string Notes { get; set; } = string.Empty;
    }

    public class UpdateMeterDto
    {
        public string MeterName { get; set; } = string.Empty;
        public string MeterNumber { get; set; } = string.Empty;
        public string Register { get; set; } = string.Empty;
        public MeterType MeterType { get; set; }
        public string MeasurementUnit { get; set; } = string.Empty;
        public decimal Multiplier { get; set; }
        public bool IsActive { get; set; }
        public DateTime? RemovalDate { get; set; }
        public string Notes { get; set; } = string.Empty;
    }
}
