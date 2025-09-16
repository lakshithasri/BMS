using KlarityLive.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlarityLive.Application.Features.MeterReading.Commands
{
    public class CreateMeterReadingCommand: IRequest<bool>
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
        public string MeasurementUnit { get; set; }
        public decimal? UtilityBillAmount { get; set; }
        public string UtilityBillUnit { get; set; }
        public string Source { get; set; }
        public string Notes { get; set; }
    }
}
