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
public static HttpResponseData Run(
    [HttpTrigger(AuthorizationLevel.Function, "get", "post", "put", "delete")] HttpRequestData req,
    FunctionContext context)
{
    var services = new ServiceCollection();
    ConfigureDependencies(services);

    var serviceProvider = services.BuildServiceProvider();
    var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
    var peopleService = serviceProvider.GetRequiredService<PeopleService>();

    var response = req.CreateResponse(HttpStatusCode.OK);
    try
    {
        switch (req.Method)
        {
            case "POST":
                using (StreamReader reader = new StreamReader(req.Body, System.Text.Encoding.UTF8))
                {
                    var json = reader.ReadToEnd();
                    var person = JsonSerializer.Deserialize<PeopleService.Person>(json);
                    var res = peopleService.Add(person.FirstName, person.LastName);
                    response.WriteAsJsonAsync(res);
                }
                break;
            case "PUT":
                using (StreamReader putReader = new StreamReader(req.Body, System.Text.Encoding.UTF8))
                {
                    var putJson = putReader.ReadToEnd();
                    var putPerson = JsonSerializer.Deserialize<PeopleService.Person>(putJson);
                    var putResult = peopleService.Update(putPerson.Id, putPerson.FirstName, putPerson.LastName);
                    response.WriteAsJsonAsync(putResult);
                }
                break;
            case "GET":
                var people = peopleService.Get();
                response.WriteAsJsonAsync(people);
                break;
            case "DELETE":
                using (StreamReader deleteReader = new StreamReader(req.Body, System.Text.Encoding.UTF8))
                {
                    var deleteJson = deleteReader.ReadToEnd();
                    var deletePerson = JsonSerializer.Deserialize<PeopleService.Person>(deleteJson);
                    peopleService.Delete(deletePerson.Id);
                    response.WriteAsJsonAsync(new { Message = "Delete operation successful." });
                }
                break;
            default:
                response = req.CreateResponse(HttpStatusCode.BadRequest);
                break;
        }
    }
    catch (Exception ex)
    {
        var logger = loggerFactory.CreateLogger("FunctionHandler");
        var errorMessage = $"An error occurred while processing the request. Details: {ex.Message}";

        logger.LogError(ex, errorMessage);

        response = req.CreateResponse(HttpStatusCode.InternalServerError);
        response.WriteAsJsonAsync(new { Message = errorMessage });
    }

    return response;
}




        private static void ConfigureDependencies(IServiceCollection services)
        {
            string connectionString = "Server=tcp:dyakovenkoserver.database.windows.net,1433;Initial Catalog=dyakovenkodb;Persist Security Info=False;User ID=dyakovenko;Password=h22ZD1#yRzV;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(connectionString)
            );
            services.AddScoped<PeopleService>();
        }
    }
}
