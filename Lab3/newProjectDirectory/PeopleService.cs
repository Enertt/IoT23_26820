namespace CdvAzure.Functions
{
    public class PeopleService
    {
        private List<Person> people { get; } = new List<Person>();

        public Person Add(string firstName, string lastName)
        {
            var person = new Person
            {
                FirstName = firstName,
                LastName = lastName,
                Id = people.Count + 1
            };
            people.Add(person);
            return person;
        }

        public Person Update(int id, string firstName, string lastName)
        {
            var person = people.First(w => w.Id == id);
            person.FirstName = firstName;
            person.LastName = lastName;

            return person;
        }

        public void Delete(int id)
        {
            var person = people.First(w => w.Id == id);
            people.Remove(person);
        }

        public Person Find(int id)
        {
            return people.First(w => w.Id == id);
        }

        public IEnumerable<Person> Get()
        {
            return people;
        }
    }

    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}