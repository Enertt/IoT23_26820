using Lab1.DTO;

namespace Lab1.Rest
{
    public interface IPeopleService
    {
        IEnumerable<Person> GetPeople();
    }
}