// =============================================================================
// RULE ID   : cr-dotnet-0037
// RULE NAME : Synchronous HttpClient
// CATEGORY  : Memory
// DESCRIPTION: FIXED - Converted to async/await pattern for cloud scalability
// =============================================================================

using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SyntheticLegacyApp.Memory
{
    public class ExternalApiClient
    {
        private readonly HttpClient _httpClient = new HttpClient();

        // FIXED: Converted to async/await pattern
        public async Task<string> GetUserProfile(int userId)
        {
            var response = await _httpClient
                .GetAsync($"https://api.internal.corp/users/{userId}");

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        // FIXED: Converted to async/await pattern
        public async Task<bool> SubmitOrder(string orderJson)
        {
            var content = new StringContent(orderJson, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://api.internal.corp/orders", content);

            return response.IsSuccessStatusCode;
        }

        // FIXED: Converted to async/await pattern
        public async Task<byte[]> DownloadReport(string reportId)
        {
            return await _httpClient
                .GetByteArrayAsync($"https://reports.internal.corp/download/{reportId}");
        }

        // FIXED: Converted to async/await pattern
        public async Task<string> GetAuthToken(string clientId, string secret)
        {
            var formData = new StringContent(
                $"client_id={clientId}&client_secret={secret}&grant_type=client_credentials",
                Encoding.UTF8, "application/x-www-form-urlencoded");

            var tokenResponse = await _httpClient
                .PostAsync("https://auth.internal.corp/token", formData);

            return await tokenResponse.Content.ReadAsStringAsync();
        }

        // FIXED: Converted to async/await with parallel processing for better throughput
        public async Task SyncBulkFetch(int[] ids)
        {
            var tasks = ids.Select(async id =>
            {
                string result = await _httpClient
                    .GetStringAsync($"https://api.internal.corp/items/{id}");

                Console.WriteLine($"Fetched item {id}: {result.Length} bytes");
            });

            await Task.WhenAll(tasks);
        }
    }
}
