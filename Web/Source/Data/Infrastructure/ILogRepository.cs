namespace Home.Source.Data.Infrastructure
{
    public interface ILogRepository
    {
        Task SaveLogAsync(string comment);
    }
}