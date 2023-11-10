namespace Home.Source.Services.Time
{
    public class TimeService : ITimeService
    {
        private readonly ILogger<TimeService> logger;

        public TimeService(ILogger<TimeService> logger)
        {
            this.logger = logger;
        }

        public void PrintTime()
        {
            logger?.LogInformation(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
        }
    }
}
