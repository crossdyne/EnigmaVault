namespace EnigmaVault.Web.Bff.Extensions
{
    public static class RedisKeyExtensions
    {
        public static string SessionKey(string sessionId) => $"crossdyne:sessions:{sessionId}";
        public static string DistributedLock(string sessionId) => $"crossdyne:shared:lock:sessions:{sessionId}";
        public static string DataProtectionKeys() => "crossdyne:shared:bff:data-protection-keys";
    }
}