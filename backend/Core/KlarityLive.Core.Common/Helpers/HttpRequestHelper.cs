using KlarityLive.Domain.Core.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;

namespace KlarityLive.Core.Common.Helpers
{
    public static class HttpRequestHelper
    {
        public static async Task<string> ReadRequestBodyAsync(HttpRequest req)
        {
            using var reader = new StreamReader(req.Body);
            return await reader.ReadToEndAsync();
        }

        public static int GetUserIdFromTokenAsync(HttpRequest req)
        {
            var userId = -1;
            var token = GetToken(req);

            try
            {
                // Example for JWT token
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadJwtToken(token);
                userId = int.Parse(jsonToken.Claims!.FirstOrDefault(x => x.Type == "extension_UserId")?.Value ?? "-1");
            }
            catch (Exception ex)
            {

            }

            return userId;
        }
        public static string? GetToken(HttpRequest req)
        {
            var authHeader = req.Headers.FirstOrDefault(h => h.Key.Equals("Authorization", StringComparison.OrdinalIgnoreCase));

            if (string.IsNullOrWhiteSpace(authHeader.Value))
            {
                throw new UnauthorizedException("Token could not be found.");
            }

            var token = authHeader.Value.First()?.Replace("Bearer ", "");
            return token;
        }

        public static string? GetTokenHash(HttpRequest req)
        {
            var authHeader = req.Headers.FirstOrDefault(h => h.Key.Equals("Authorization", StringComparison.OrdinalIgnoreCase));

            if (string.IsNullOrWhiteSpace(authHeader.Value))
            {
                throw new BadRequestException("Token could not be found.");
            }

            var token = authHeader.Value.First()?.Replace("Bearer ", "");
            return GetTokenHash(token);
        }

        public static string GetTokenHash(string? token)
        {
            if (token == null)
            {
                throw new BadRequestException("Token could not be found.");
            }
            var handler = new JwtSecurityTokenHandler();
            string tokenHash = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(token)));
            return tokenHash;
        }
        public static async Task<T> GetPayloadAsync<T>(HttpRequest req)
        {
            var body = await ReadRequestBodyAsync(req);

            if (body == null || string.IsNullOrEmpty(body))
            {
                throw new BadRequestException("Request body is empty");
            }

            return JsonConvert.DeserializeObject<T>(body)
                 ?? throw new BadRequestException("Failed to deserialize request body");
        }

        //public static AuditLogDto GetAuditLogDto<TRequest, TResponse>(
        //    HttpRequest req,
        //    TRequest? dto,
        //    TResponse? response,
        //    int? userId)
        //{

        //    var ipAddress = "";

        //    if (req.Headers["Referer"].FirstOrDefault() != null)
        //    {
        //        ipAddress = req.Headers["Referer"].FirstOrDefault() != null
        //          ? req.Headers["Referer"].FirstOrDefault()
        //          : req.Headers["Origin"].FirstOrDefault();
        //    }

        //    var audotLogDto = new AuditLogDto
        //    {
        //        Url = req.GetDisplayUrl(),
        //        HttpMethod = req.Method?.ToString() ?? "",
        //        IpAddress = ipAddress ?? "",
        //        RequestPayload = dto != null ? JsonConvert.SerializeObject(dto) : null,
        //        ResponsePayload = response != null ? JsonConvert.SerializeObject(response) : null,
        //        UserId = userId,
        //    };

        //    return audotLogDto;
        //}

        //public static AuditLogDto GetAuditLogDto<TRequest, TResponse>(
        //    HttpRequestData req,
        //    TRequest? dto,
        //    TResponse? response,
        //    int? userId)
        //{

        //    var ipAddress = "";

        //    //if (req.Headers.GetValues("X-Forwarded-For").FirstOrDefault() != null
        //    //    && req.Headers.GetValues("origin").FirstOrDefault() != null)
        //    //{
        //    //    ipAddress = req.Headers.GetValues("X-Forwarded-For").FirstOrDefault() != null
        //    //      ? req.Headers.GetValues("X-Forwarded-For").FirstOrDefault()?.Split(',').FirstOrDefault()?.Trim()
        //    //      : req.Headers.GetValues("origin").FirstOrDefault();
        //    //}

        //    var audotLogDto = new AuditLogDto
        //    {
        //        Url = req.Url != null ? req.Url.AbsoluteUri : "",
        //        HttpMethod = req.Method?.ToString() ?? "",
        //        IpAddress = ipAddress ?? "",
        //        RequestPayload = dto != null ? JsonConvert.SerializeObject(dto) : null,
        //        ResponsePayload = response != null ? JsonConvert.SerializeObject(response) : null,
        //        UserId = userId,
        //    };

        //    return audotLogDto;
        //}

        //public static int GetLodgementUserIdFromHeader(HttpRequest req)
        //{
        //    return int.Parse(GetHeaderValue(req, HeaderNames.LodgementUserIdHeaderName));
        //}

        public static string GetHeaderValue(HttpRequest req, string headerName)
        {
            var header = req.Headers.FirstOrDefault(h => h.Key.Equals(headerName, StringComparison.OrdinalIgnoreCase));

            if (string.IsNullOrWhiteSpace(header.Value))
            {
                throw new BadRequestException("Token could not be found.");
            }

            return header.Value.First() ?? "";
        }

        public static void ValidateUserIdLodgementId(int userId, int lodgementId)
        {
            if (userId <= 0)
            {
                throw new BadRequestException($"UserId is invalid: {userId}");
            }

            if (lodgementId <= 0)
            {
                throw new BadRequestException($"LodgementId is invalid: {lodgementId}");
            }
        }

        public static string GetClientIpAddress(this HttpRequest request)
        {
            // Try to get the real IP from various headers
            var headers = request.Headers;

            if (headers.TryGetValue("X-Forwarded-For", out var forwardedFor))
            {
                return forwardedFor.FirstOrDefault()?.Split(',')[0].Trim();
            }

            if (headers.TryGetValue("X-Real-IP", out var realIp))
            {
                return realIp.FirstOrDefault();
            }

            if (headers.TryGetValue("CF-Connecting-IP", out var cloudflareIp))
            {
                return cloudflareIp.FirstOrDefault();
            }

            return request.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "unknown";
        }

        /// <summary>
        /// Validates the HMAC signature of the request body
        /// </summary>
        /// <param name="payload">The request body</param>
        /// <param name="providedSignature">The signature from the request header</param>
        /// <param name="secretKey">The secret key shared with the vendor</param>
        /// <returns>True if the signature is valid, false otherwise</returns>
        public static bool ValidateHmacSignature(string payload, string providedSignature, string secretKey)
        {
            try
            {
                // Convert the secret key to bytes
                byte[] keyBytes = Encoding.UTF8.GetBytes(secretKey);

                // Convert the payload to bytes
                byte[] messageBytes = Encoding.UTF8.GetBytes(payload);

                // Create HMAC-SHA256 hash
                using (HMACSHA256 hmac = new HMACSHA256(keyBytes))
                {
                    // Compute hash
                    byte[] hashBytes = hmac.ComputeHash(messageBytes);

                    // Convert hash to Base64 string or hex string depending on vendor specifications
                    // Option 1: Base64 (more common)
                    string computedSignature = Convert.ToBase64String(hashBytes);

                    // Option 2: Hex string
                    // string computedSignature = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

                    // Compare the computed signature with the provided signature
                    return string.Equals(computedSignature, providedSignature, StringComparison.Ordinal);
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
