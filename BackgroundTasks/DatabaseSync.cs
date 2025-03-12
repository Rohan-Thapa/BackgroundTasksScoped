using SriLibs.DataSync.MSSQLSERVER;

namespace BackgroundTasks
{
    public class DatabaseSync
    {
        // the connection string between the database. Here it is only one-way but should have done two-way later
        const string backupdb = "Server=AculanServer;Database=backupdb;TrustServerCertificate=True;";
        const string primarydb = "Server=AculanServer;Database=CitizenDB;TrustServerCertificate=True;";
        public static void Synchorinzation()
        {
            Console.WriteLine($"Trying to write something on `{backupdb}` from `{primarydb}`");
            // For it to work there should be the installation of the package `SriLibs.DataSync.MSSQLSERVER`
            Initializer initializer = new();
            Synchronizer synchronizer = initializer.Initiate(primarydb, backupdb); // choosing all the tables
            SyncInfo info = synchronizer.FlashSync(SyncDirections.SyncToServer);

            Console.WriteLine("Inserted : {0}\nUpdated : {1}\nDeleted : {2}\nSyncDuration : {3}\nStatus : {4}",
                info.Inserted, info.Updated, info.Deleted, info.Duration.ToString(), info.Status);
            
            Console.WriteLine("The sync of the database is done!!!!");
        }
    }
}
