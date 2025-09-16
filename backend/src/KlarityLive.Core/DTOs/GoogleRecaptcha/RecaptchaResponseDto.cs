using Newtonsoft.Json;

namespace KlarityLive.Core.DTOs.GoogleRecaptcha
{
    public class RecaptchaResponseDto
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("challenge_ts")]
        public DateTime ChallengeTimestamp { get; set; }

        [JsonProperty("hostname")]
        public string Hostname { get; set; }

        [JsonProperty("error-codes")]
        public string[] ErrorCodes { get; set; }

        [JsonProperty("score")]
        public decimal? Score { get; set; } // For reCAPTCHA v3

        [JsonProperty("action")]
        public string Action { get; set; } // For reCAPTCHA v3
    }
}
