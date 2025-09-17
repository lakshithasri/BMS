namespace KlarityLive.Core.Common.DTOs.DataInjestion
{
    public class ExcelImportResultDto
    {
        public bool IsSuccess { get; set; }
        public int SuccessCount { get; set; }
        public int ErrorCount { get; set; }
        public List<string> Errors { get; set; }
    }
}
