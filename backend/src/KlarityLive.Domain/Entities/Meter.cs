using KlarityLive.Domain.Entities.Base;
using KlarityLive.Domain.Enums;

namespace KlarityLive.Domain.Entities
{
    public record Meter : BaseEntity
    {
        public int PropertyId { get; set; }
        public string MeterName { get; set; }
        public string MeterNumber { get; set; }
        public string Register { get; set; }
        public MeterType MeterType { get; set; } // Electricity, Gas, Water, etc.
        public string MeasurementUnit { get; set; } // kWh, m3, etc.
        public decimal Multiplier { get; set; } = 1.0m;
        public bool IsActive { get; set; } = true;
        public DateTime InstallationDate { get; set; }
        public DateTime? RemovalDate { get; set; }
        public string Notes { get; set; }
        public virtual Building Building { get; set; }
        public virtual ICollection<MeterReading> MeterReadings { get; set; }
    }
}
