using ClosedXML.Excel;
using KlarityLive.Application.DataProcessing.Services.Interfaces;
using KlarityLive.Core.Common.DTOs.DataInjestion;
using KlarityLive.Domain.Core.Entities.BMS;
using KlarityLive.Domain.Core.Enums;
using KlarityLive.Infrastructure.Data.Repositories.Base.Interfaces;

namespace KlarityLive.Application.DataProcessing.Services.Concrete
{
    public class ExcelUploaderService(IUnitOfWork unitOfWork) : IExcelUploaderService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<ExcelImportResultDto> UploadExcelDataAsync(Stream excelStream, string fileName)
        {
            var result = new ExcelImportResultDto();

            try
            {
                using var workbook = new XLWorkbook(excelStream);
                var worksheet = workbook.Worksheet(1);

                var rows = ParseExcelRows(worksheet);

                foreach (var row in rows)
                {
                    try
                    {
                        await ProcessRowAsync(row);
                        result.SuccessCount++;
                    }
                    catch (Exception ex)
                    {
                        result.Errors.Add($"Row error: {ex.Message}");
                        result.ErrorCount++;
                    }
                }

                await _unitOfWork.SaveChangesAsync();
                result.IsSuccess = result.ErrorCount == 0;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Errors.Add($"File processing error: {ex.Message}");
            }

            return result;
        }

        private List<ExcelImportRowDto> ParseExcelRows(IXLWorksheet worksheet)
        {
            var rows = new List<ExcelImportRowDto>();
            var rowCount = worksheet.RowCount();

            // Assuming first row contains headers
            for (int row = 2; row <= rowCount; row++)
            {
                var importRow = new ExcelImportRowDto
                {
                    Category = GetCellValue(worksheet, row, 1),
                    Tenancy = GetCellValue(worksheet, row, 2),
                    TenantOwner = GetCellValue(worksheet, row, 3),
                    Lease = GetCellValue(worksheet, row, 4),
                    MeterName = GetCellValue(worksheet, row, 5),
                    Register = GetCellValue(worksheet, row, 6),
                    FromDate = GetDateValue(worksheet, row, 7),
                    ToDate = GetDateValue(worksheet, row, 8),
                    Days = GetIntValue(worksheet, row, 9),
                    PrevReading = GetDecimalValue(worksheet, row, 10),
                    PresReading = GetDecimalValue(worksheet, row, 11),
                    Advance = GetNullableDecimalValue(worksheet, row, 12),
                    Multiplier = GetDecimalValue(worksheet, row, 13, 1.0m),
                    Quantity = GetDecimalValue(worksheet, row, 14),
                    MeasurementUnit = GetCellValue(worksheet, row, 15),
                    Building = GetCellValue(worksheet, row, 16),
                    Address = GetCellValue(worksheet, row, 17),
                    Suburb = GetCellValue(worksheet, row, 18),
                    Note = GetCellValue(worksheet, row, 19),
                    UtilityBillUnit = GetCellValue(worksheet, row, 20)
                };

                rows.Add(importRow);
            }

            return rows;
        }

        private async Task ProcessRowAsync(ExcelImportRowDto row)
        {
            // 1. Create or find Property
            var property = await GetOrCreatePropertyAsync(row.Building, row.Address, row.Suburb);

            // 2. Create or find Tenant
            var tenant = await GetOrCreateTenantAsync(row.TenantOwner);

            // 3. Create or find Tenancy
            var tenancy = await GetOrCreateTenancyAsync(tenant.Id, row.Tenancy, row.Lease, row.Category);

            // 4. Create or find Meter
            var meter = await GetOrCreateMeterAsync(property.Id, row.MeterName, row.Register, row.MeasurementUnit, row.Multiplier);

            // 5. Create MeterReading
            var meterReading = new MeterReading
            {
                //MeterId = meter.Id,
                //TenancyId = tenancy.Id,
                //ReadingDate = row.ToDate,
                //PeriodFromDate = row.FromDate,
                //PeriodToDate = row.ToDate,
                //PeriodDays = row.Days,
                //PreviousReading = row.PrevReading,
                //PresentReading = row.PresReading,
                //Advance = row.Advance,
                //Multiplier = row.Multiplier,
                //Quantity = row.Quantity,
                //MeasurementUnit = row.MeasurementUnit,
                //UtilityBillUnit = row.UtilityBillUnit,
                //Source = ReadingSource.ExcelImport,
                //Notes = row.Note,
                CreatedOn = DateTime.UtcNow
            };

            await _unitOfWork.MeterReadingRepository.AddAsync(meterReading);
        }

        private async Task<Building> GetOrCreatePropertyAsync(string name, string address, string suburb)
        {
            var property = await _unitOfWork.BuildingRepository
                .FirstOrDefaultAsync(p => p.Name == name && p.Address == address);

            if (property == null)
            {
                property = new Building
                {
                    Name = name,
                    Address = address,
                    Suburb = suburb,
                    CreatedOn = DateTime.UtcNow
                };

                await _unitOfWork.BuildingRepository.AddAsync(property);
            }

            return property;
        }

        private async Task<Tenant> GetOrCreateTenantAsync(string name)
        {
            var tenant = await _unitOfWork.TenantRepository
                .FirstOrDefaultAsync(t => t.Name == name);

            if (tenant == null)
            {
                tenant = new Tenant
                {
                    Name = name,
                    TenantType = DetermineTenantType(name),
                    CreatedOn = DateTime.UtcNow
                };

                await _unitOfWork.TenantRepository.AddAsync(tenant);
            }

            return tenant;
        }

        private async Task<Tenancy> GetOrCreateTenancyAsync(int tenantId, string tenancyRef, string leaseRef, string category)
        {
            var tenancy = await _unitOfWork.TenancyRepository
                .FirstOrDefaultAsync(t => t.LeaseReference == leaseRef && t.Category == category);

            if (tenancy == null)
            {
                tenancy = new Tenancy
                {
                    LeaseReference = leaseRef,
                    Category = category,
                    LeaseStartDate = DateTime.UtcNow, // You might want to derive this from data
                    Status = TenancyStatus.Active,
                    CreatedOn = DateTime.UtcNow,
                    UpdatedOn = DateTime.UtcNow
                };

                await _unitOfWork.TenancyRepository.AddAsync(tenancy);

                await CreateTenantTenancyAsync(tenantId, tenancy.Id);
            }

            return tenancy;
        }

        private async Task CreateTenantTenancyAsync(int tenantId, int tenancyId)
        {
            var tenantTenancy = new TenantTenancy
            {
                TenancyId = tenancyId,
                TenantId = tenantId,
                CreatedOn = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow
            };

            await _unitOfWork.TenantTenancyRepository.AddAsync(tenantTenancy);
        }

        private async Task<Meter> GetOrCreateMeterAsync(int buildingId,
            string meterName,
            string register,
            string unit,
            decimal multiplier)
        {
            var meter = await _unitOfWork.MeterRepository
                .FirstOrDefaultAsync(m => m.BuildingId == buildingId && m.MeterName == meterName && m.Register == register);

            if (meter == null)
            {
                meter = new Meter
                {
                    BuildingId = buildingId,
                    MeterName = meterName,
                    Register = register,
                    MeasurementUnit = unit,
                    Multiplier = multiplier,
                    MeterType = DetermineMeterType(unit),
                    InstallationDate = DateTime.UtcNow,
                    IsActive = true,
                    CreatedOn = DateTime.UtcNow
                };

                await _unitOfWork.MeterRepository.AddAsync(meter);
            }

            return meter;
        }

        // Helper methods
        private string GetCellValue(IXLWorksheet worksheet, int row, int col)
        {
            return worksheet.Cell(row, col)?.Value.GetText() ?? string.Empty;
        }

        private DateTime GetDateValue(IXLWorksheet worksheet, int row, int col)
        {
            var cellValue = worksheet.Cell(row, col)?.GetDateTime();
            if (cellValue is DateTime dateTime)
                return dateTime;
            if (DateTime.TryParse(cellValue?.ToString(), out var parsedDate))
                return parsedDate;
            return DateTime.MinValue;
        }

        private int GetIntValue(IXLWorksheet worksheet, int row, int col, int defaultValue = 0)
        {
            var cellValue = worksheet.Cell(row, col).GetValue<int>();
            if (int.TryParse(cellValue.ToString(), out var result))
                return result;
            return defaultValue;
        }

        private decimal GetDecimalValue(IXLWorksheet worksheet, int row, int col, decimal defaultValue = 0m)
        {
            var cellValue = worksheet.Cell(row, col).GetDouble();
            if (decimal.TryParse(cellValue.ToString(), out var result))
                return result;
            return defaultValue;
        }

        private decimal? GetNullableDecimalValue(IXLWorksheet worksheet, int row, int col)
        {
            var cellValue = worksheet.Cell(row, col).GetDouble();
            if (decimal.TryParse(cellValue.ToString(), out var result))
                return result;
            return null;
        }

        private TenantType DetermineTenantType(string name)
        {
            if (name.ToLower().Contains("owner"))
                return TenantType.Owner;
            if (name.ToLower().Contains("common"))
                return TenantType.CommonArea;
            return TenantType.Tenant;
        }

        private MeterType DetermineMeterType(string unit)
        {
            if (unit?.ToLower().Contains("kwh") == true || unit?.ToLower().Contains("electricity") == true)
                return MeterType.Electricity;
            if (unit?.ToLower().Contains("m3") == true || unit?.ToLower().Contains("gas") == true)
                return MeterType.Gas;
            if (unit?.ToLower().Contains("water") == true || unit?.ToLower().Contains("litre") == true)
                return MeterType.Water;
            return MeterType.Other;
        }
    }
}
