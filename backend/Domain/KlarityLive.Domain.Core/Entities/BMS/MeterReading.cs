using KlarityLive.Domain.Core.Entities.Base;

namespace KlarityLive.Domain.Core.Entities.BMS
{
    public class MeterReading : BaseEntity
    {
        public int MeterId { get; set; }
        public DateTime ReadingDate { get; set; }
        public Meter Meter { get; set; }
    }
}
