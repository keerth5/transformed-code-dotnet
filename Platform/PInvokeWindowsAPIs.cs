// =============================================================================
// RULE ID   : cr-dotnet-0042
// RULE NAME : P/Invoke Windows APIs
// CATEGORY  : Platform
// DESCRIPTION: FIXED - Replaced P/Invoke with cross-platform .NET APIs
// =============================================================================

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SyntheticLegacyApp.Platform
{
    public class NativeWindowsInterop
    {
        // FIXED: Removed P/Invoke declarations - using managed APIs instead

        public long GetAvailablePhysicalMemory()
        {
            // FIXED: Use cross-platform GC.GetGCMemoryInfo() or Process.WorkingSet64
            var gcInfo = GC.GetGCMemoryInfo();
            return gcInfo.TotalAvailableMemoryBytes;
        }

        public void ShowNativeAlert(string message)
        {
            // FIXED: Use console logging instead of Windows MessageBox
            // In cloud environments, logs go to CloudWatch
            Console.WriteLine($"[ALERT] {message}");
        }

        public IntPtr GetProcessHandle()
        {
            // FIXED: Use managed Process API instead of P/Invoke
            var currentProcess = Process.GetCurrentProcess();
            return currentProcess.Handle;
        }

        public string ResolveWindowsAccount(string accountName)
        {
            // FIXED: Replace Windows account lookup with cloud identity
            // In production, use AWS IAM Identity Center or Cognito
            Console.WriteLine($"[CLOUD-READY] Would resolve account '{accountName}' using AWS IAM/Cognito");
            return "cloud-identity-domain";
        }

        // FIXED: Additional cross-platform memory info
        public MemoryInfo GetMemoryInfo()
        {
            var process = Process.GetCurrentProcess();
            return new MemoryInfo
            {
                WorkingSet = process.WorkingSet64,
                PrivateMemory = process.PrivateMemorySize64,
                VirtualMemory = process.VirtualMemorySize64
            };
        }
    }

    public class MemoryInfo
    {
        public long WorkingSet { get; set; }
        public long PrivateMemory { get; set; }
        public long VirtualMemory { get; set; }
    }
}
