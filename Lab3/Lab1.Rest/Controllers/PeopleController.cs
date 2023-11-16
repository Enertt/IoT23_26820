using Lab1.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Lab1.Rest
{
    [ApiController]
    [Route("people")]
    public class PeopleController : ControllerBase
    {
        private readonly ILogger<PeopleController> logger;
        private readonly IPeopleService peopleService;

        public PeopleController(ILogger<PeopleController> logger, IPeopleService peopleService)
        {
            this.logger = logger;
            this.peopleService = peopleService;
        }

        [HttpGet]
        public IEnumerable<Person> GetPeople()
        {
            return peopleService.GetPeople();
        }

        [HttpGet("{id}")]
        public Person FindPerson([FromRoute] int id)
        {
            return peopleService.FindPerson(id);
        }




        [HttpPost]
        public Person AddPerson([FromBody] Person person)
        {
            return peopleService.AddPerson(person);
        }
    }
}