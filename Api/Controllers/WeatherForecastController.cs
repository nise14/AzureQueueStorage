using System.Text.Json;
using Azure.Storage.Queues;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly QueueClient _queueClient;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, QueueClient queueClient)
    {
        _logger = logger;
        _queueClient = queueClient;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [HttpPost]
    public async Task Post(WeatherForecast data)
    {
        // var connectionString = "DefaultEndpointsProtocol=https;AccountName=youtubevideodemo;AccountKey=apipMQSrcSUe1dqvXEuGUZOVSRwjcvTFakZ0gn+R+MmlpqyfB3PEt+1gMr9XT3fOAjiVftY7Wqyy+AStkQD5rA==;EndpointSuffix=core.windows.net";
        // var queueName = "add-weatherdata";
        // var queueClient = new QueueClient(connectionString, queueName);
        var message = JsonSerializer.Serialize(data);
        await _queueClient.SendMessageAsync(message, null, TimeSpan.FromSeconds(-1));
    }
}
