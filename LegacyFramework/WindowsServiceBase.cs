// =============================================================================
// RULE ID   : cr-dotnet-0060
// RULE NAME : Windows Service Base
// CATEGORY  : LegacyFramework
// DESCRIPTION: FIXED - Migrated to BackgroundService for cloud deployment
// =============================================================================

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace SyntheticLegacyApp.LegacyFramework
{
    // FIXED: Replaced ServiceBase with BackgroundService for cloud-native hosting
    public class OrderProcessingService : BackgroundService
    {
        private readonly TimeSpan _processingInterval = TimeSpan.FromSeconds(30);

        // FIXED: Use ExecuteAsync instead of OnStart/OnStop
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("OrderProcessingService started (cloud-native)");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await ProcessOrders(stoppingToken);
                    await Task.Delay(_processingInterval, stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    // Expected when stopping
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing orders: {ex.Message}");
                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                }
            }

            Console.WriteLine("OrderProcessingService stopped (cloud-native)");
        }

        private async Task ProcessOrders(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return;
            
            Console.WriteLine("Processing pending orders...");
            // TODO: Implement actual order processing logic
            await Task.CompletedTask;
        }

        // FIXED: Override StopAsync for graceful shutdown
        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("OrderProcessingService stopping gracefully...");
            await base.StopAsync(cancellationToken);
        }
    }

    // FIXED: Use Generic Host instead of ServiceBase.Run
    public class WindowsServiceEntryPoint
    {
        public static async Task Main(string[] args)
        {
            // FIXED: Use Generic Host for cloud-native hosting
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<OrderProcessingService>();
                })
                .Build();

            await host.RunAsync();
        }
    }
}
