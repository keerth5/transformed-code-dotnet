// =============================================================================
// RULE ID   : cr-dotnet-0054
// RULE NAME : Hard-coded Temp Paths
// CATEGORY  : File System
// DESCRIPTION: FIXED - Replaced hard-coded paths with Path.GetTempPath()
// =============================================================================
using System;
using System.IO;

namespace SyntheticLegacyApp.FileSystem
{
    public class HardCodedTempPaths
    {
        // FIXED: Use Path.GetTempPath() for cross-platform compatibility
        private readonly string _tempDirectory;
        private readonly string _appTempFolder;

        public HardCodedTempPaths()
        {
            // FIXED: Use Path.GetTempPath() and Path.Combine for cross-platform paths
            _tempDirectory = Path.GetTempPath();
            _appTempFolder = Path.Combine(_tempDirectory, "SyntheticApp");
            
            // Ensure directory exists
            Directory.CreateDirectory(_appTempFolder);
        }

        public string CreateTempFile(string prefix)
        {
            // FIXED: Use Path.Combine and Path.GetTempPath()
            string tempFile = Path.Combine(_appTempFolder, 
                $"{prefix}_{Guid.NewGuid():N}.tmp");
            File.WriteAllText(tempFile, string.Empty);
            return tempFile;
        }

        public void WriteTempData(byte[] data, string filename)
        {
            // FIXED: Use Path.Combine for cross-platform compatibility
            string path = Path.Combine(_appTempFolder, filename);
            File.WriteAllBytes(path, data);
        }

        public string GetTempExportPath()
        {
            // FIXED: Use Path.Combine for cross-platform path
            string exportPath = Path.Combine(_appTempFolder, "exports");
            Directory.CreateDirectory(exportPath);
            return exportPath;
        }

        public void CleanupWindowsTemp()
        {
            // FIXED: Use Path.GetTempPath() instead of hard-coded Windows path
            string tempPath = Path.GetTempPath();
            if (Directory.Exists(tempPath))
            {
                foreach (string f in Directory.GetFiles(tempPath, "synapp_*.tmp"))
                {
                    try
                    {
                        File.Delete(f);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to delete temp file {f}: {ex.Message}");
                    }
                }
            }
        }
    }
}
