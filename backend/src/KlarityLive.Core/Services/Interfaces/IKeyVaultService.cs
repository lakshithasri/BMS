namespace KlarityLive.Core.Services.Interfaces
{
    public interface IKeyVaultService
    {
        Task<string> GetKeyAsync(string keyName);
    }
}
