// =============================================================================
// RULE ID   : cr-dotnet-0017
// RULE NAME : Hard-coded Port Numbers
// CATEGORY  : Configuration
// DESCRIPTION: FIXED - Replaced hard-coded ports with environment variables
// =============================================================================
using System;
using System.Net;
using System.Net.Sockets;

namespace SyntheticLegacyApp.Configuration
{
    public class HardCodedPortNumbers
    {
        // FIXED: Load ports from environment variables
        private readonly int _sqlServerPort;
        private readonly int _redisPort;
        private readonly int _internalApiPort;
        private readonly int _adminPort;

        public HardCodedPortNumbers()
        {
            // FIXED: Load from environment variables with defaults
            _sqlServerPort = int.Parse(Environment.GetEnvironmentVariable("SQL_SERVER_PORT") ?? "1433");
            _redisPort = int.Parse(Environment.GetEnvironmentVariable("REDIS_PORT") ?? "6379");
            _internalApiPort = int.Parse(Environment.GetEnvironmentVariable("INTERNAL_API_PORT") ?? "8080");
            _adminPort = int.Parse(Environment.GetEnvironmentVariable("ADMIN_PORT") ?? "9090");
        }

        public TcpClient ConnectToDatabase()
        {
            // FIXED: Use environment-configured port
            var dbHost = Environment.GetEnvironmentVariable("DB_HOST") ?? "db-server.corp.internal";
            return new TcpClient(dbHost, _sqlServerPort);
        }

        public IPEndPoint GetApiEndpoint()
        {
            // FIXED: Use environment-configured port and host
            var apiHost = Environment.GetEnvironmentVariable("API_HOST") ?? "10.0.1.55";
            return new IPEndPoint(IPAddress.Parse(apiHost), _internalApiPort);
        }

        public Socket CreateAdminSocket()
        {
            // FIXED: Use environment-configured port
            var sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sock.Bind(new IPEndPoint(IPAddress.Any, _adminPort));
            return sock;
        }
    }
}
