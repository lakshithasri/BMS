using KlarityLive.Application.Services.Interfaces;
using KlarityLive.Core.DTOs.GoogleRecaptcha;
using KlarityLive.Core.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace KlarityLive.Core.Services.Concrete
{
    public class GoogleRecaptchaService(
         IKeyVaultService keyVaultService,
         IHttpClientFactory httpClientFactory,
         ILogger<GoogleRecaptchaService> logger) : IGoogleRecaptchaService
    {
        private readonly IKeyVaultService _keyVaultService = keyVaultService;
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient("GoogleRecaptcha");
        private readonly ILogger<GoogleRecaptchaService> _logger = logger;

        public async Task<RecaptchaResponseDto> VerifyRecaptchaAsync(string token, string remoteIp = null)
        {
            try
            {
                _logger.LogInformation("Verifying reCAPTCHA token");

                var secretKey = await _keyVaultService.GetKeyAsync("GoogleReCaptchaSecretKey");

                var parameters = new Dictionary<string, string>
                {
                    ["secret"] = secretKey,
                    ["response"] = token
                };

                if (!string.IsNullOrEmpty(remoteIp))
                {
                    parameters["remoteip"] = remoteIp;
                }

                var content = new FormUrlEncodedContent(parameters);
                var response = await _httpClient.PostAsync(
                    "https://www.google.com/recaptcha/api/siteverify",
                    content);

                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var recaptchaResponse = JsonConvert.DeserializeObject<RecaptchaResponseDto>(responseContent);

                _logger.LogInformation("reCAPTCHA verification result: {Success}", recaptchaResponse.Success);

                if (!recaptchaResponse.Success && recaptchaResponse.ErrorCodes?.Length > 0)
                {
                    _logger.LogWarning("reCAPTCHA errors: {Errors}", string.Join(", ", recaptchaResponse.ErrorCodes));
                }

                return recaptchaResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verifying reCAPTCHA token");
                return new RecaptchaResponseDto { Success = false };
            }
        }

        public async Task<bool> IsValidAsync(string token, string remoteIp = null, decimal minimumScore = 0.5m)
        {
            var response = await VerifyRecaptchaAsync(token, remoteIp);

            if (!response.Success)
                return false;

            // Check score if it exists (reCAPTCHA v3)
            if (response.Score.HasValue)
            {
                return response.Score.Value >= minimumScore;
            }

            return true; // reCAPTCHA v2 doesn't have score
        }
    }
}
