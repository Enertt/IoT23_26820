using Lab1.DTO;
using Lab1.Rest.Database;
using Lab1.Rest.Database.Entities;

namespace Lab1.Rest.Services
{
    public class DatabasePeopleService : IPeopleService
    {
        private readonly PeopleDb db;

        public DatabasePeopleService(PeopleDb db)
        {
            this.db = db;
        }

        public Person AddPerson(Person person)
        {
            var entity = new PersonEntity
            {
                FirstName = person.FirstName,
                LastName = person.LastName
            };
            db.People.Add(entity);
            db.SaveChanges();
            person.Id = entity.Id;
            return person;
        }

        public Person FindPerson(int id)
        {
            var person = db.People.First(w => w.Id == id);
            return MapToDTO(person);
        }

        public IEnumerable<Person> GetPeople()
        {
            var peopleList = db.People.Select(s => MapToDTO(s));

            return peopleList;
        }

        public Person MapToDTO(PersonEntity entity)
        {
            return new Person
            {
                FirstName = entity.FirstName,
                Id = entity.Id,
                LastName = entity.LastName
            };
        }
    }
}