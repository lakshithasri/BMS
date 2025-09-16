using KlarityLive.Application.Features.Users.Commands.CreateUser;
using KlarityLive.Application.Services.Interfaces;
using KlarityLive.Domain.Entities.Cosmos;
using KlarityLive.Domain.Enums;
using KlarityLive.Infrastructure.Data.Context;
using KlarityLive.Infrastructure.Data.Repositories.Base.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlarityLive.Application.Features.MeterReading.Commands
{
    public class CreateMeterReadingCommandHandller
    {
        public class CreateMeterReadingCommandHandler : IRequestHandler<CreateMeterReadingCommand, bool>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly ICosmosDbService _cosmosDbService;
            private readonly KlarityLiveDbContext _context;
            private readonly ILogger<CreateUserCommandHandler> _logger;

            public CreateMeterReadingCommandHandler(IUnitOfWork unitOfWork, ICosmosDbService cosmosDbService, KlarityLiveDbContext context, ILogger<CreateUserCommandHandler> logger)
            {
                _unitOfWork = unitOfWork;
                _cosmosDbService = cosmosDbService;
                _context = context;
                _logger = logger;
            }

            public async Task<bool> Handle(CreateMeterReadingCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    // 1️⃣ Save relational entity (SQL via UoW)
                    var meterReading = new Domain.Entities.MeterReading
                    {
                        MeterId = request.MeterId,
                        TenancyId = request.TenancyId,
                        ReadingDate = request.ReadingDate,
                        PeriodFromDate = request.PeriodFromDate,
                        PeriodToDate = request.PeriodToDate,
                        PeriodDays = request.PeriodDays,
                        PreviousReading = request.PreviousReading,
                        PresentReading = request.PresentReading,
                        Advance = request.Advance,
                        Multiplier = request.Multiplier,
                        Quantity = request.Quantity,
                        MeasurementUnit = request.MeasurementUnit,
                        UtilityBillAmount = request.UtilityBillAmount,
                        UtilityBillUnit = request.UtilityBillUnit,
                        Source = Enum.TryParse<ReadingSource>(request.Source, out var sourceEnum) ? sourceEnum : ReadingSource.Manual,
                        Notes = request.Notes
                    };

                    await _unitOfWork.MeterReadingRepository.AddAsync(meterReading);

                    // 2️⃣ Save time-series snapshot (Cosmos)
                    var doc = new MeterReadingDocument
                    {
                        MeterId = meterReading.MeterId,
                        TenancyId = meterReading.TenancyId,
                        ReadingDate = meterReading.ReadingDate,
                        PeriodFromDate = meterReading.PeriodFromDate,
                        PeriodToDate = meterReading.PeriodToDate,
                        PeriodDays = meterReading.PeriodDays,
                        PreviousReading = meterReading.PreviousReading,
                        PresentReading = meterReading.PresentReading,
                        Advance = meterReading.Advance,
                        Multiplier = meterReading.Multiplier,
                        Quantity = meterReading.Quantity,
                        MeasurementUnit = meterReading.MeasurementUnit,
                        UtilityBillAmount = meterReading.UtilityBillAmount,
                        UtilityBillUnit = meterReading.UtilityBillUnit,
                        Source = meterReading.Source.ToString(),
                        Notes = meterReading.Notes
                    };

                    await _cosmosDbService.CreateMeterReadingAsync(doc);

                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating meter reading");
                    throw;
                }
            }
        }

    }
}
