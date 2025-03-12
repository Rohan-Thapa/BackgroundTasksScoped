
using System.Runtime.CompilerServices;

namespace BackgroundTasks
{
    public class MyBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<MyBackgroundService> _logger;
        public MyBackgroundService(IServiceProvider serviceProvider, ILogger<MyBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    _logger.LogInformation("From MyBackgroundService: ExecuteAsync {datetime}", DateTime.Now);
                    var scopedService = scope.ServiceProvider.GetRequiredService<IScopedService>();
                    scopedService.Write();  // here by default the service is in Singleton while we injected the Scoped ones here.
                    await Task.Delay(TimeSpan.FromSeconds(65), stoppingToken);
                }
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("From MyBackgroundService: StopAsync {datetime}", DateTime.Now);
            return base.StopAsync(cancellationToken);
        }
    }
}
