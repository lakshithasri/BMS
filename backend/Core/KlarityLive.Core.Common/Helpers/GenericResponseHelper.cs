using KlarityLive.Core.Common.DTOs.ApiResponse;

namespace KlarityLive.Core.Common.Helpers
{
    public static class GenericResponseHelper
    {
        public static ApiResponse<T> GenerateResponse<T>(int statusCode, string type, T data = default, string message = "", List<ApiError>? errors = null)
        {
            return new ApiResponse<T>(statusCode, type, data)
            {
                Data = data,
                Errors = errors,
                Message = message
            };
        }

        public static ApiPagedResponse<T> GeneratePagedResponse<T>(int statusCode,
            int totalCount,
            int pageSize,
            int pageNumber,
            int totalPages,
            string type,
            T data,
            string message)
        {
            return new ApiPagedResponse<T>(statusCode, totalCount, pageSize, pageNumber, totalPages, type, data, message);
        }

        public static WorksheetApiPagedResponse<T> GenerateWorksheetPagedResponse<T>(int statusCode,
           int totalCount,
           int pageSize,
           int pageNumber,
           int totalPages,
           string worksheetLabel,
           decimal totalSum,
           T data,
           string message)
        {
            return new WorksheetApiPagedResponse<T>(statusCode, totalCount, pageSize, pageNumber, totalPages, worksheetLabel, totalSum, data, message);
        }

        public static ApiPagedResponse<T> GeneratePagedResponse<T>(int statusCode, string message, List<ApiError>? errors)
        {
            return new ApiPagedResponse<T>(statusCode, message: message, errors: errors);
        }

        public static ApiResponse<T> GenerateResponse<T>(int statusCode)
        {

            return new ApiResponse<T>(statusCode, "", default);
        }

        public static ApiResponse<T> GenerateResponse<T>(int statusCode, string? message = null, List<ApiError>? errors = null)
        {
            return new ApiResponse<T>(statusCode, "", default, message, errors);
        }
    }
}
