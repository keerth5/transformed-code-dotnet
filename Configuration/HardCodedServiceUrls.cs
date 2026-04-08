// =============================================================================
// RULE ID   : cr-dotnet-0011
// RULE NAME : Hard-coded Service URLs
// CATEGORY  : Configuration
// DESCRIPTION: FIXED - Externalized URLs to environment variables and AWS Systems Manager
// =============================================================================
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SyntheticLegacyApp.Configuration
{
    public class HardCodedServiceUrls
    {
        // FIXED: Load URLs from environment variables instead of hard-coding
        private readonly string _paymentServiceUrl;
        private readonly string _inventoryServiceUrl;
        private readonly string _authServiceUrl;
        private readonly string _reportingApiUrl;

        public HardCodedServiceUrls()
        {
            // Load from environment variables (can be populated from AWS Systems Manager Parameter Store)
            _paymentServiceUrl = Environment.GetEnvironmentVariable("PAYMENT_SERVICE_URL") 
                ?? throw new InvalidOperationException("PAYMENT_SERVICE_URL environment variable not set");
            _inventoryServiceUrl = Environment.GetEnvironmentVariable("INVENTORY_SERVICE_URL") 
                ?? throw new InvalidOperationException("INVENTORY_SERVICE_URL environment variable not set");
            _authServiceUrl = Environment.GetEnvironmentVariable("AUTH_SERVICE_URL") 
                ?? throw new InvalidOperationException("AUTH_SERVICE_URL environment variable not set");
            _reportingApiUrl = Environment.GetEnvironmentVariable("REPORTING_API_URL") 
                ?? throw new InvalidOperationException("REPORTING_API_URL environment variable not set");
        }

        public async Task<string> GetPaymentStatus(string paymentId)
        {
            // FIXED: Use environment-configured URL
            using (var client = new HttpClient())
                return await client.GetStringAsync(_paymentServiceUrl + "status/" + paymentId);
        }

        public string BuildInventoryEndpoint(string productId)
        {
            return _inventoryServiceUrl + "product/" + productId; // FIXED: Uses environment variable
        }
    }
}
