using KlarityLive.Application.Services.Interfaces;
using KlarityLive.Core.DTOs.Dashboard;
using MediatR;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlarityLive.Application.Features.MeterReading.Commands
{
    public class GetMeterReadingsByDateRangeQueryHandler : IRequestHandler<GetMeterReadingsByDateRangeQuery, List<MeterReadingDto>> 
    {
        private readonly ICosmosDbService _cosmosDbService;

        public GetMeterReadingsByDateRangeQueryHandler(ICosmosDbService cosmosDbService)
        {
            _cosmosDbService = cosmosDbService;
        }

        public async Task<List<MeterReadingDto>> Handle(GetMeterReadingsByDateRangeQuery request, CancellationToken cancellationToken)
        {
            // Build Cosmos SQL query
            var query = new QueryDefinition(
                "SELECT * FROM c WHERE c.PeriodFromDate >= @fromDate AND c.PeriodToDate <= @toDate ORDER BY c.PeriodFromDate"
            )
            .WithParameter("@fromDate", request.PeriodFromDate)
            .WithParameter("@toDate", request.PeriodToDate);

            var readings = await _cosmosDbService.QueryItemsAsync<MeterReadingDto>(query, cancellationToken);

            return readings.ToList();
        }
    }
}
