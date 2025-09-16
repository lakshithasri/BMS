using KlarityLive.Application.Services.Interfaces;
using KlarityLive.Domain.Entities.Cosmos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlarityLive.Application.Features.MeterReading.Commands
{
    public class GetMeterReadingsByMeterIdQueryHandler
        : IRequestHandler<GetMeterReadingsByMeterIdQuery, IEnumerable<MeterReadingDocument>>
    {
        private readonly ICosmosDbService _cosmosDbService;

        public GetMeterReadingsByMeterIdQueryHandler(ICosmosDbService cosmosDbService)
        {
            _cosmosDbService = cosmosDbService;
        }

        public async Task<IEnumerable<MeterReadingDocument>> Handle(
            GetMeterReadingsByMeterIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _cosmosDbService.GetMeterReadingsByMeterIdAsync(request.MeterId);
        }
    }
}
