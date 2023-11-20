namespace Home.Source.Services.WorkerX
{
    public class BackgroundWorkerService1 : IHostedService
    {
        private readonly ILogger<BackgroundWorkerService1> logger;

        public BackgroundWorkerService1(ILogger<BackgroundWorkerService1> logger)
        {
            this.logger = logger;            
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Service started");

            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(1000, cancellationToken);
                logger.LogInformation("Worker running at {time}", DateTimeOffset.Now);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Service stopped");
            return Task.CompletedTask;
        }
    }
}
