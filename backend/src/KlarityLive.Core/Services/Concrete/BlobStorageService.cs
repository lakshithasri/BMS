using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using KlarityLive.Core.Security;
using KlarityLive.Core.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace KlarityLive.Core.Services.Concrete
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly BlobContainerClient _containerClient;
        private readonly ILogger<BlobStorageService> _logger;
        private readonly ICachingService _cache;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly AppSecrets _appSecrets;
        private readonly string _accountName = "";
        private readonly string _accountKey = "";
        private readonly string _baseUrl = "";

        public BlobStorageService(BlobContainerClient containerClient,
            BlobServiceClient blobServiceClient,
            ICachingService cache,
            AppSecrets appSecrets,
            ILogger<BlobStorageService> logger)
        {
            _appSecrets = appSecrets;

            var parts = _appSecrets.BlobStorageConnectionString.Split(';');

            foreach (var part in parts)
            {
                if (part.StartsWith("AccountName="))
                {
                    _accountName = part.Substring("AccountName=".Length);
                }
                else if (part.StartsWith("AccountKey="))
                {
                    _accountKey = part.Substring("AccountKey=".Length);
                }
            }

            _baseUrl = Environment.GetEnvironmentVariable("_baseBlobUrl") ?? throw new ArgumentException("_baseBlobUrl");
            _containerClient = containerClient ?? throw new ArgumentException(nameof(containerClient));
            _logger = logger ?? throw new ArgumentException(nameof(logger)); ;
            _cache = cache ?? throw new ArgumentException(nameof(cache)); ;
            _blobServiceClient = blobServiceClient ?? throw new ArgumentException(nameof(blobServiceClient)); ;
        }

        /// <summary>
        /// Uploads a file to Azure Blob Storage
        /// </summary>
        /// <param name="containerName">Name of the blob container</param>
        /// <param name="fileName">Name of the file to be uploaded</param>
        /// <param name="filePath">Full path to the file on local system</param>
        /// <returns>URL of the uploaded blob</returns>
        public async Task<string> UploadFileAsync(string containerName, string fileName, Stream stream)
        {
            try
            {
                // Create a BlobServiceClient
                //var blobServiceClient = new BlobServiceClient(_connectionString);

                // Get a reference to the container
                var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

                // Create the container if it doesn't exist
                await containerClient.CreateIfNotExistsAsync();

                // Get a reference to the blob
                var blobClient = containerClient.GetBlobClient(fileName);

                // Upload the file
                await blobClient.UploadAsync(stream, overwrite: true);

                return blobClient.Uri.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error uploading file to blob storage: {ex.Message}", ex.StackTrace);
                throw new Exception($"Error uploading file to blob storage: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Downloads a file from Azure Blob Storage
        /// </summary>
        /// <param name="containerName">Name of the blob container</param>
        /// <param name="fileName">Name of the file to download</param>
        /// <param name="destinationPath">Full path where the file should be saved</param>
        /// <returns>True if download is successful</returns>
        public async Task<byte[]> DownloadFileAsByteArrayAsync(string containerName, string fileName)
        {
            try
            {
                var downloadResult = await DownloadFileAsync(containerName, fileName);
                return downloadResult.Content.ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error uploading file to blob storage: {ex.Message}", ex.StackTrace);
                throw new Exception($"Error downloading file from blob storage: {ex.Message}", ex);
            }
        }

        private async Task<BlobDownloadResult> DownloadFileAsync(string containerName, string fileName)
        {

            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

            var blobClient = containerClient.GetBlobClient(fileName);

            if (!await blobClient.ExistsAsync())
            {
                throw new FileNotFoundException($"File {fileName} not found in container {containerName}");
            }
            var downloadResult = await blobClient.DownloadContentAsync();

            return downloadResult!;
        }

        public async Task<string> DownloadFileAsStringAsync(string containerName, string fileName)
        {
            try
            {
                var downloadResult = await DownloadFileAsync(containerName, fileName);
                var fileContent = downloadResult.Content.ToString();
                return fileContent;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error uploading file to blob storage: {ex.Message}", ex.StackTrace);
                throw new Exception($"Error downloading file from blob storage: {ex.Message}", ex);
            }
        }

        public string GetFileUrlWithSasAsync(string blobContainerName, string blobName)
        {

            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = blobContainerName,
                BlobName = blobName,
                Resource = "b",
                StartsOn = DateTimeOffset.UtcNow,
                ExpiresOn = DateTimeOffset.UtcNow.AddHours(1)
            };
            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            var sasToken = sasBuilder.ToSasQueryParameters(new StorageSharedKeyCredential(_accountName, _accountKey)).ToString();

            return $"{_baseUrl}/{blobContainerName}/{blobName}?{sasToken}";
        }


        public string GetFileUrlWithSasFromUrl(string blobUrl)
        {
            var uri = new Uri(blobUrl);

            // Assuming the format: https://<account>.blob.core.windows.net/<container>/<blobname>
            var segments = uri.AbsolutePath.TrimStart('/').Split('/');
            if (segments.Length < 2)
                throw new ArgumentException("Invalid blob URL");

            var blobContainerName = segments[0];
            var blobName = string.Join('/', segments.Skip(1)); // handle folders in blob path

            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = blobContainerName,
                BlobName = blobName,
                Resource = "b",
                StartsOn = DateTimeOffset.UtcNow,
                ExpiresOn = DateTimeOffset.UtcNow.AddHours(2)
            };
            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            var sasToken = sasBuilder.ToSasQueryParameters(new StorageSharedKeyCredential(_accountName, _accountKey)).ToString();

            return $"{_baseUrl}/{blobContainerName}/{blobName}?{sasToken}";
        }

        public async Task<bool> BlobExistsAsync(string containerName, string blobName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(blobName);
            return await blobClient.ExistsAsync();
        }

    }
}
