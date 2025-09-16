using KlarityLive.Core.DTOs.Dashboard;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlarityLive.Application.Features.MeterReading.Commands
{
    public class GetMeterReadingsByDateRangeQuery : IRequest<List<MeterReadingDto>>
    {
        public DateTime PeriodFromDate { get; set; }
        public DateTime PeriodToDate { get; set; }

        public GetMeterReadingsByDateRangeQuery(DateTime periodFromDate, DateTime periodToDate)
        {
            PeriodFromDate = periodFromDate;
            PeriodToDate = periodToDate;
        }
    }

}
