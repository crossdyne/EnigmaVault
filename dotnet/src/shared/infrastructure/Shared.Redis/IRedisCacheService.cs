namespace Shared.Redis
{
    public interface IRedisCacheService
    {
        Task<bool> RemoveAsync(string key);
        Task<bool> SetJsonAsync<T>(string key, T value, TimeSpan? expiry = null);
        Task<T?> GetJsonAsync<T>(string key);
        Task<bool> SetRemoveAsync(string key, string value);
    }
}