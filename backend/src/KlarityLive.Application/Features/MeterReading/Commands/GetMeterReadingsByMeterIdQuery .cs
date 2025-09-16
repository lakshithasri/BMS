using KlarityLive.Domain.Entities.Cosmos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlarityLive.Application.Features.MeterReading.Commands
{
    public class GetMeterReadingsByMeterIdQuery : IRequest<IEnumerable<MeterReadingDocument>>
    {
        public int MeterId { get; set; }

        public GetMeterReadingsByMeterIdQuery(int meterId)
        {
            MeterId = meterId;
        }
    }
}
