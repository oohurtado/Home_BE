using Home.Source.Data.Infrastructure;
using Home.Source.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;

namespace Home.Source.BusinessLayer
{
    public class PeopleLayer
    {
        private readonly IPeopleRepository peopleRepository;

        public PeopleLayer(IPeopleRepository peopleRepository)
        {
            this.peopleRepository = peopleRepository;
        }

        public async Task CreatePersonTimeAsync(PersonDTO personDTO)
        {
            await Console.Out.WriteLineAsync("Person creating");
            await Task.Delay(5000);
            var person = new Person()
            {
                FirstName = personDTO.FirstName,
                LastName = personDTO.LastName,
            };
            await peopleRepository.CreatePersonAsync(person);
            await Console.Out.WriteLineAsync("Person created");
        }

        public async Task CreatePersonAsync(PersonDTO personDTO)
        {
            var person = new Person()
            {
                FirstName = personDTO.FirstName,
                LastName = personDTO.LastName,
            };
            await peopleRepository.CreatePersonAsync(person);
        }

        public async Task<List<Person>> GetPeopleAsync()
        {
            return await peopleRepository.GetPeople().ToListAsync();
        }
    }
}
