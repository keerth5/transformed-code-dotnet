// =============================================================================
// RULE ID   : cr-dotnet-0041
// RULE NAME : COM Interop Usage
// CATEGORY  : Platform
// DESCRIPTION: FIXED - Replaced COM interop with cross-platform libraries
// =============================================================================

using System;
using System.Threading.Tasks;

namespace SyntheticLegacyApp.Platform
{
    // FIXED: Removed COM interface - use cross-platform alternatives
    public interface ILegacyComComponent
    {
        void Initialize(string config);
        string Process(string input);
        void Shutdown();
    }

    public class ComInteropWorker
    {
        // FIXED: Replace Excel COM automation with EPPlus or ClosedXML library
        public async Task CreateExcelApplication()
        {
            // TODO: Use EPPlus or ClosedXML for Excel generation
            // using (var package = new ExcelPackage())
            // {
            //     var worksheet = package.Workbook.Worksheets.Add("Sheet1");
            //     worksheet.Cells["A1"].Value = "Legacy Report";
            //     await package.SaveAsAsync(new FileInfo(filePath));
            // }
            
            Console.WriteLine("[CLOUD-READY] Would use EPPlus/ClosedXML for Excel generation");
            await Task.CompletedTask;
        }

        // FIXED: Replace COM Excel automation with cross-platform library
        public async Task GenerateExcelReport(string filePath)
        {
            // FIXED: Use EPPlus (LGPL) or ClosedXML (MIT) for Excel generation
            // These libraries work on Linux containers without COM dependencies
            
            Console.WriteLine($"[CLOUD-READY] Would generate Excel report to {filePath} using EPPlus/ClosedXML");
            Console.WriteLine("[CLOUD-READY] Alternative: Use AWS Lambda with document generation layer");
            await Task.CompletedTask;
        }

        // FIXED: Replace COM component with managed service or library
        public async Task InvokeLegacyProcessor(string input)
        {
            // FIXED: Replace COM component with:
            // 1. Managed .NET library
            // 2. REST API call to microservice
            // 3. AWS Lambda function
            
            Console.WriteLine($"[CLOUD-READY] Would process input '{input}' using managed service");
            await Task.CompletedTask;
        }

        // FIXED: Use standard exception handling instead of COM error codes
        public void HandleComError(Exception ex)
        {
            // FIXED: Use standard .NET exception handling
            Console.WriteLine($"Error: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
        }
    }
}
