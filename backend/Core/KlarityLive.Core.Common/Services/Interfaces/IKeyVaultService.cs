namespace KlarityLive.Core.Common.Services.Interfaces
{
    public interface IKeyVaultService
    {
        Task<string> GetKeyAsync(string keyName);
    }
}
