namespace KlarityLive.Core.DTOs.ExcelUpload
{
    public class ImportResultDto
    {
        public bool IsSuccess { get; set; }
        public int SuccessCount { get; set; }
        public int ErrorCount { get; set; }
        public List<string> Errors { get; set; } = new List<string>();

        public string Summary => $"Import completed. Success: {SuccessCount}, Errors: {ErrorCount}";

    }
}
