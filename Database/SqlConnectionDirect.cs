// =============================================================================
// RULE ID   : cr-dotnet-0013
// RULE NAME : SqlConnection Direct Usage
// CATEGORY  : Database
// DESCRIPTION: FIXED - Use proper connection management with using statements
// =============================================================================
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SyntheticLegacyApp.Database
{
    public class SqlConnectionDirect
    {
        private readonly string _connectionString;

        public SqlConnectionDirect()
        {
            // FIXED: Load connection string from environment variable
            _connectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING") 
                ?? throw new InvalidOperationException("DATABASE_CONNECTION_STRING not set");
        }

        public async Task InsertOrder(string orderId, decimal amount)
        {
            // FIXED: Use 'using' statement for proper connection disposal
            // FIXED: Use async methods for better scalability
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var cmd = new SqlCommand("INSERT INTO Orders (OrderId, Amount) VALUES (@id, @amt)", connection))
                {
                    cmd.Parameters.AddWithValue("@id", orderId);
                    cmd.Parameters.AddWithValue("@amt", amount);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            // Connection automatically closed and returned to pool
        }

        public async Task<DataTable> QueryOrders(string status)
        {
            // FIXED: Use 'using' statement and parameterized query to prevent SQL injection
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (var cmd = new SqlCommand("SELECT * FROM Orders WHERE Status=@status", conn))
                {
                    cmd.Parameters.AddWithValue("@status", status);
                    using (var adapter = new SqlDataAdapter(cmd))
                    {
                        var dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
            // Connection automatically closed and returned to pool
        }

        // FIXED: Additional method showing best practices with Entity Framework Core
        // In production, replace direct SqlConnection with EF Core + RDS Proxy
        public void MigrateToEntityFramework()
        {
            Console.WriteLine("[CLOUD-READY] In production, use Entity Framework Core with RDS Proxy");
            Console.WriteLine("[CLOUD-READY] RDS Proxy provides connection pooling and IAM authentication");
        }
    }
}
