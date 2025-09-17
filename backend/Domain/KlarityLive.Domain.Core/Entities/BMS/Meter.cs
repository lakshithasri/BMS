using KlarityLive.Domain.Core.Entities.Base;
using KlarityLive.Domain.Core.Enums;

namespace KlarityLive.Domain.Core.Entities.BMS
{
    public class Meter : BaseEntity, ISoftDelete
    {
        public int BuildingId { get; set; }
        public string MeterName { get; set; }
        public string MeterNumber { get; set; }
        public string Register { get; set; }
        public MeterType MeterType { get; set; }
        public string MeasurementUnit { get; set; }
        public decimal Multiplier { get; set; }
        public bool IsActive { get; set; }
        public DateTime InstallationDate { get; set; }
        public DateTime RemovalDate { get; set; }
        public string Notes { get; set; }
        public bool IsDeleted { get; set; }

        public Building Building { get; set; }
        public List<MeterReading> MeterReadings { get; set; }
    }
}
