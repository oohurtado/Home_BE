using Home.Source.DataBase;
using Home.Source.Models.Entities;
using Home.Source.Models;
using Microsoft.Extensions.DependencyInjection;
using Home.Source.Data.Infrastructure;

namespace Home.Source.Data.Repositories
{
    public class LogRepository : ILogRepository
    {
        private readonly IServiceProvider serviceProvider;

        public LogRepository(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task SaveLogAsync(string comment)
        {
            await using var scope = serviceProvider.CreateAsyncScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

            await Task.Delay(1000);
            var log = new Log
            {
                Comment = comment,
            };
            dbContext.Logs.Add(log);
            await dbContext.SaveChangesAsync();
        }
    }
}
