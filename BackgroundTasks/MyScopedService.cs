namespace BackgroundTasks
{
    public class MyScopedService : IScopedService
    {
        private readonly ILogger<MyScopedService> _logger;

        public Guid Id { get; set; }

        public MyScopedService(ILogger<MyScopedService> logger)
        {
            _logger = logger;
            Id = Guid.NewGuid();
        }
        public void Write()
        {
            _logger.LogInformation("MyScopedService `Database Sync` {Id}", Id);
            DatabaseSync.Synchorinzation();
        }
    }

    public interface IScopedService
    {
        void Write();
    }
}
