using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlarityLive.Core.DTOs.Dashboard
{
    public class MeterReadingDto
    {
        public int MeterId { get; set; }
        public int? TenancyId { get; set; }
        public DateTime ReadingDate { get; set; }
        public DateTime PeriodFromDate { get; set; }
        public DateTime PeriodToDate { get; set; }
        public decimal Quantity { get; set; }
        public decimal? UtilityBillAmount { get; set; }
        public string MeasurementUnit { get; set; }
    }

}
