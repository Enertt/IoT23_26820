using System.Net;
using System.Text.Json;
using Azure;
using CdvAzure.Functions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace CdvAzure.Function
{
    public class PeopleFN
    {
        private readonly ILogger _logger;
        private readonly PeopleService peopleService;

        public PeopleFN(ILoggerFactory loggerFactory, PeopleService peopleService)
        {
            _logger = loggerFactory.CreateLogger<PeopleFN>();
            this.peopleService = peopleService;
        }

        [Function("PeopleFN")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", "put", "delete")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            switch (req.Method)
            {
                case "POST":
                    StreamReader reader = new StreamReader(req.Body, System.Text.Encoding.UTF8);
                    var json = reader.ReadToEnd();
                    var person = JsonSerializer.Deserialize<Person>(json);
                    var res = peopleService.Add(person.FirstName, person.LastName);
                    response.WriteAsJsonAsync(res);
                    break;
                case "PUT":
                    StreamReader putReader = new StreamReader(req.Body, System.Text.Encoding.UTF8);
                    var putJson = putReader.ReadToEnd();
                    var putPerson = JsonSerializer.Deserialize<Person>(putJson);
                    var putResult = peopleService.Update(putPerson.Id, putPerson.FirstName, putPerson.LastName);
                    response.WriteAsJsonAsync(putResult);
                    break;
                case "GET":
                    var people = peopleService.Get();
                    response.WriteAsJsonAsync(people);
                    break;
                case "DELETE":
                    StreamReader deleteReader = new StreamReader(req.Body, System.Text.Encoding.UTF8);
                    var deleteJson = deleteReader.ReadToEnd();
                    var deletePerson = JsonSerializer.Deserialize<Person>(deleteJson);
                    peopleService.Delete(deletePerson.Id);
                    response.WriteAsJsonAsync(new { Message = "Delete operation successful." });
                    break;
            }
          return response;
        }
    }
}
