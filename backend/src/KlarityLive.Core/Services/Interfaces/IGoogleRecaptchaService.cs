using KlarityLive.Core.DTOs.GoogleRecaptcha;

namespace KlarityLive.Application.Services.Interfaces
{
    public interface IGoogleRecaptchaService
    {
        Task<RecaptchaResponseDto> VerifyRecaptchaAsync(string token, string remoteIp = null);
        Task<bool> IsValidAsync(string token, string remoteIp = null, decimal minimumScore = 0.5m);
    }
}
