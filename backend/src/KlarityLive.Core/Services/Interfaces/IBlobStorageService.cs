namespace KlarityLive.Core.Services.Interfaces
{
    public interface IBlobStorageService
    {
        Task<string> UploadFileAsync(string containerName, string fileName, Stream stream);
        Task<byte[]> DownloadFileAsByteArrayAsync(string containerName, string fileName);
        Task<string> DownloadFileAsStringAsync(string containerName, string fileName);
        string GetFileUrlWithSasAsync(string blobContainerName, string blobName);
        string GetFileUrlWithSasFromUrl(string blobUrl);
        Task<bool> BlobExistsAsync(string blobContainerName, string blobName);
    }
}
