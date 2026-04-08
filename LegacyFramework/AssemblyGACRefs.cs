// =============================================================================
// RULE ID   : cr-dotnet-0049
// RULE NAME : Assembly GAC References
// CATEGORY  : LegacyFramework
// DESCRIPTION: FIXED - Package assemblies with application instead of GAC
// =============================================================================

using System;
using System.IO;
using System.Reflection;

namespace SyntheticLegacyApp.LegacyFramework
{
    // FIXED: Load assemblies from application directory instead of GAC
    public class GACAssemblyLoader
    {
        private readonly string _assemblyDirectory;

        public GACAssemblyLoader()
        {
            // FIXED: Load from application directory or configured path
            _assemblyDirectory = Environment.GetEnvironmentVariable("ASSEMBLY_PATH") 
                ?? Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        // FIXED: Load assembly from local directory instead of GAC
        public Assembly LoadReportViewerFromGac()
        {
            // FIXED: Load from bin directory - assembly packaged with application
            var assemblyPath = Path.Combine(_assemblyDirectory, "Microsoft.ReportViewer.Common.dll");
            return File.Exists(assemblyPath) 
                ? Assembly.LoadFrom(assemblyPath)
                : throw new FileNotFoundException($"Assembly not found: {assemblyPath}");
        }

        // FIXED: Load type from local assembly
        public Type GetReportViewerType()
        {
            Assembly asm = LoadReportViewerFromGac();
            return asm.GetType("Microsoft.Reporting.WinForms.ReportViewer");
        }

        // FIXED: Load Crystal Reports from local directory
        public Assembly LoadCrystalRuntimeFromGac()
        {
            // FIXED: Load from bin directory - assembly packaged with application
            var assemblyPath = Path.Combine(_assemblyDirectory, "CrystalDecisions.CrystalReports.Engine.dll");
            return File.Exists(assemblyPath)
                ? Assembly.LoadFrom(assemblyPath)
                : throw new FileNotFoundException($"Assembly not found: {assemblyPath}");
        }

        // FIXED: Get version from local assembly
        public string GetGacAssemblyVersion(string assemblyFileName)
        {
            try
            {
                var assemblyPath = Path.Combine(_assemblyDirectory, assemblyFileName);
                if (!File.Exists(assemblyPath))
                {
                    Console.WriteLine($"Assembly not found: {assemblyPath}");
                    return null;
                }

                Assembly asm = Assembly.LoadFrom(assemblyPath);
                return asm.GetName().Version?.ToString() ?? "unknown";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Assembly load failed for '{assemblyFileName}': {ex.Message}");
                return null;
            }
        }

        // FIXED: Check local assemblies instead of GAC
        public void LogGacDependencies()
        {
            string[] localAssemblies =
            {
                "Microsoft.ReportViewer.Common.dll",
                "Microsoft.VisualBasic.PowerPacks.Vs.dll",
                "Interop.ADODB.dll"
            };

            foreach (var fileName in localAssemblies)
            {
                var assemblyPath = Path.Combine(_assemblyDirectory, fileName);
                if (File.Exists(assemblyPath))
                {
                    Console.WriteLine($"[OK] {fileName}");
                }
                else
                {
                    Console.WriteLine($"[MISSING] {fileName}");
                }
            }
        }
    }
}
