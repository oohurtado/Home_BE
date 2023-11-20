using System.Threading;

namespace Home.Source.Services.WorkerX
{
    public class BackgroundWorkerService2 : BackgroundService
    {
        private readonly ILogger<BackgroundWorkerService1> logger;

        public BackgroundWorkerService2(ILogger<BackgroundWorkerService1> logger)
        {
            this.logger = logger;            
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
                logger.LogInformation("Worker running at {time}", DateTimeOffset.Now);
            }
        }
    }
}
