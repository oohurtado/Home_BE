using Home.Source.Data.Infrastructure;
using Home.Source.DataBase;
using Home.Source.Models.Entities;

namespace Home.Source.Data.Repositories
{
    public class PeopleRepository : IPeopleRepository
    {
        private readonly DatabaseContext databaseContext;

        public PeopleRepository(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        public async Task CreatePersonAsync(Person person)
        {
            databaseContext.Add(person);
            await databaseContext.SaveChangesAsync();
        }

        public IQueryable<Person> GetPeople()
        {
            return databaseContext.People.AsQueryable();
        }
    }
}
