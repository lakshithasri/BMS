namespace KlarityLive.Core.Common.DTOs.ApiResponse
{
    public class ApiResponse<T>(int statusCode, string type, T? data, string? message = null, List<ApiError>? errors = null)
    {
        public int StatusCode { get; set; } = statusCode;
        public string? Message { get; set; } = message;
        public string? Type { get; set; } = type;
        public T Data { get; set; } = data;
        public List<ApiError>? Errors { get; set; } = errors ?? [];
    }

    public class ApiError(string code, string message)
    {
        public string? Code { get; set; } = code;
        public string? Message { get; set; } = message;
    }

    public class ApiPagedResponse<T>(int statusCode,
        int totalCount = 0,
        int pageSize = 0,
        int pageNumber = 0,
        int totalPages = 0,
        string type = "",
        T data = default,
        string? message = null,
        List<ApiError>? errors = null)
    {
        public int StatusCode { get; set; } = statusCode;
        public string? Message { get; set; } = message;
        public int TotalCount { get; set; } = totalCount;
        public int PageSize { get; set; } = pageSize;
        public int CurrentPage { get; set; } = pageNumber;
        public int TotalPages { get; set; } = totalPages;
        public string Type { get; set; } = type;
        public T Data { get; set; } = data;
        public List<ApiError>? Errors { get; set; } = errors ?? [];
    }

    public class WorksheetApiPagedResponse<T>(int statusCode,
        int totalCount = 0,
        int pageSize = 0,
        int pageNumber = 0,
        int totalPages = 0,
        string worksheetLabel = null,
        decimal totalSum = 0,
        T data = default,
        string? message = null,
        List<ApiError>? errors = null)
    {
        public int StatusCode { get; set; } = statusCode;
        public string? Message { get; set; } = message;
        public int TotalCount { get; set; } = totalCount;
        public int PageSize { get; set; } = pageSize;
        public int CurrentPage { get; set; } = pageNumber;
        public int TotalPages { get; set; } = totalPages;
        public string WorksheetLabel { get; set; } = worksheetLabel;
        public decimal TotalSum { get; set; } = totalSum;
        public T Data { get; set; } = data;
        public List<ApiError>? Errors { get; set; } = errors ?? [];
    }
}
