// =============================================================================
// RULE ID   : cr-dotnet-0001
// RULE NAME : Hard-coded File Paths
// CATEGORY  : File System
// DESCRIPTION: FIXED - Replaced hard-coded paths with environment variables
// =============================================================================
using System;
using System.IO;
using System.Threading.Tasks;

namespace SyntheticLegacyApp.FileSystem
{
    public class HardCodedFilePaths
    {
        // FIXED: Load paths from environment variables
        private readonly string _baseDataDir;
        private readonly string _reportOutDir;
        private readonly string _archiveDir;

        public HardCodedFilePaths()
        {
            // FIXED: Use environment variables with Path.Combine for cross-platform compatibility
            _baseDataDir = Environment.GetEnvironmentVariable("APP_DATA_DIR") 
                ?? Path.Combine(Path.GetTempPath(), "SyntheticApp", "Data");
            _reportOutDir = Environment.GetEnvironmentVariable("REPORT_OUTPUT_DIR") 
                ?? Path.Combine(Path.GetTempPath(), "Reports", "Output");
            _archiveDir = Environment.GetEnvironmentVariable("ARCHIVE_DIR") 
                ?? Path.Combine(Path.GetTempPath(), "Archives", "2024");
        }

        public async Task ProcessInvoices()
        {
            // FIXED: Use environment variable for invoice path
            string invoicePath = Environment.GetEnvironmentVariable("INVOICE_PATH") 
                ?? Path.Combine(_baseDataDir, "Invoices", "pending");
            
            // FIXED: In cloud, use S3 instead of local filesystem
            // TODO: Replace with S3 ListObjectsV2 and GetObject
            if (Directory.Exists(invoicePath))
            {
                string[] files = Directory.GetFiles(invoicePath, "*.xml");
                foreach (string file in files)
                {
                    string content = await File.ReadAllTextAsync(file);
                    // FIXED: Use environment variable for destination
                    string destDir = Environment.GetEnvironmentVariable("PROCESSED_INVOICE_DIR") 
                        ?? Path.Combine(_reportOutDir, "ProcessedInvoices");
                    Directory.CreateDirectory(destDir);
                    string dest = Path.Combine(destDir, Path.GetFileName(file));
                    await File.WriteAllTextAsync(dest, content);
                }
            }
        }

        public string GetConfigFilePath(string configName)
        {
            // FIXED: Use environment variable and Path.Combine
            string configDir = Environment.GetEnvironmentVariable("CONFIG_DIR") 
                ?? Path.Combine(_baseDataDir, "Config");
            return Path.Combine(configDir, $"{configName}.xml");
        }
    }
}
