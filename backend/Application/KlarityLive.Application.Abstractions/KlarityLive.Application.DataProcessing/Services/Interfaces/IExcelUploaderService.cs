using KlarityLive.Core.Common.DTOs.DataInjestion;

namespace KlarityLive.Application.DataProcessing.Services.Interfaces
{
    public interface IExcelUploaderService
    {
        Task<ExcelImportResultDto> UploadExcelDataAsync(Stream excelStream, string fileName);
    }
}
