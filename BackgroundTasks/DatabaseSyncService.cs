using System;
using System.Threading;
using System.Threading.Tasks;
using Dotmim.Sync;
using Dotmim.Sync.SqlServer;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

namespace BackgroundTasks
{
    public class DatabaseSyncService : BackgroundService
    {
        private readonly IConfiguration _configuration;

        public DatabaseSyncService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await SyncDatabases();
                Console.WriteLine("Next sync in 15 seconds..");
                await Task.Delay(TimeSpan.FromSeconds(15), stoppingToken);
            }
        }

        private async Task SyncDatabases()
        {
            Console.WriteLine("Starting database synchronization...");

            var primaryDb = _configuration.GetConnectionString("PrimaryDb");
            var backupDb = _configuration.GetConnectionString("BackupDb");

            var serverProvider = new SqlSyncProvider(primaryDb);
            var clientProvider = new SqlSyncProvider(backupDb);

            // Define which tables to sync
            var setup = new SyncSetup(new string[] { "Citizens" });

            // Sync agent
            var agent = new SyncAgent(clientProvider, serverProvider);

            // Execute sync
            var result = await agent.SynchronizeAsync(setup);
            Console.WriteLine($"Sync completed: {result.TotalChangesUploadedToServer} uploaded, {result.TotalChangesDownloadedFromServer} downloaded.");
        }
    }
}

// there should be proper file 📁 structure for the project.