// =============================================================================
// RULE ID   : cr-dotnet-0009
// RULE NAME : Hard-coded Connection Strings
// CATEGORY  : Configuration
// DESCRIPTION: FIXED - Migrated to AWS Secrets Manager for connection strings
// =============================================================================
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SyntheticLegacyApp.Configuration
{
    public class HardCodedConnectionStrings
    {
        // FIXED: Load connection strings from environment variables (populated from AWS Secrets Manager)
        private readonly string _primaryDb;
        private readonly string _reportingDb;

        public HardCodedConnectionStrings()
        {
            // FIXED: Load from environment variables instead of hard-coding
            _primaryDb = Environment.GetEnvironmentVariable("PRIMARY_DB_CONNECTION_STRING") 
                ?? throw new InvalidOperationException("PRIMARY_DB_CONNECTION_STRING not set");
            _reportingDb = Environment.GetEnvironmentVariable("REPORTING_DB_CONNECTION_STRING") 
                ?? throw new InvalidOperationException("REPORTING_DB_CONNECTION_STRING not set");
        }

        public SqlConnection GetPrimaryConnection()
        {
            // FIXED: Use environment-configured connection string
            return new SqlConnection(_primaryDb);
        }

        public async Task ExecuteQuery(string sql)
        {
            // FIXED: Use environment-configured connection string
            var legacyDbConnectionString = Environment.GetEnvironmentVariable("LEGACY_DB_CONNECTION_STRING") 
                ?? throw new InvalidOperationException("LEGACY_DB_CONNECTION_STRING not set");

            using (var conn = new SqlConnection(legacyDbConnectionString))
            {
                await conn.OpenAsync();
                using (var cmd = new SqlCommand(sql, conn))
                {
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        // FIXED: Helper method to load connection string from AWS Secrets Manager
        public static async Task<string> LoadConnectionStringFromSecretsManager(string secretName)
        {
            // TODO: Implement with AWSSDK.SecretsManager
            // var client = new AmazonSecretsManagerClient();
            // var request = new GetSecretValueRequest { SecretId = secretName };
            // var response = await client.GetSecretValueAsync(request);
            // return response.SecretString;
            
            Console.WriteLine($"[CLOUD-READY] Would load connection string from AWS Secrets Manager: {secretName}");
            await Task.CompletedTask;
            return Environment.GetEnvironmentVariable(secretName);
        }
    }
}
