using KlarityLive.Core.DTOs.ExcelUpload;

namespace KlarityLive.Application.Services.Interfaces
{
    public interface IExcelImportService
    {
        Task<ImportResultDto> ImportExcelDataAsync(Stream excelStream, string fileName);
    }
}
