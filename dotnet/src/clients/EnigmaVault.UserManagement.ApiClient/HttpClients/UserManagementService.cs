using System.Net.Http;

namespace EnigmaVault.UserManagement.ApiClient.HttpClients
{
    public sealed class UserManagementService(HttpClient httpClient)
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task Me(string accessToken)
        {

        }
    }
}