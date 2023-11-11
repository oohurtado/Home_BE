using Home.Source.Models.Entities;

namespace Home.Source.Data.Infrastructure
{
    public interface IPeopleRepository
    {
        Task CreatePersonAsync(Person person);
        IQueryable<Person> GetPeople();
    }
}