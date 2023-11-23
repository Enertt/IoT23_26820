using Lab1.DTO;

namespace Lab1.Rest.Services
{
    public class PeopleService : IPeopleService
    {
        public Person AddPerson(Person person)
        {
            throw new NotImplementedException();
        }

        public Person FindPerson(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Person> GetPeople()
        {
            var peopleList = new List<Person>();

            for (int i = 0; i < 10; i++)
            {
                peopleList.Add(new Person
                {
                    FirstName = $"db - Person first name - {i}",
                    LastName = $"Person last name - {i}",
                    Id = i + 1
                });
            }

            return peopleList;
        }
    }
}