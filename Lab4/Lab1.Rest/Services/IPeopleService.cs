using Lab1.DTO;

namespace Lab1.Rest
{
    public interface IPeopleService
    {
        Person FindPerson(int id);

        IEnumerable<Person> GetPeople();

        Person AddPerson(Person person);
    }
}