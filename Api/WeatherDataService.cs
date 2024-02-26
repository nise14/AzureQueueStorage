using System.Text.Json;
using Azure.Storage.Queues;

namespace Api;

public class WeatherDataService : BackgroundService
{
    private readonly ILogger<WeatherDataService> _logger;
    private readonly QueueClient _queueClient;

    public WeatherDataService(ILogger<WeatherDataService> logger, QueueClient queueClient)
    {
        _logger = logger;
        _queueClient = queueClient;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // var connectionString = "DefaultEndpointsProtocol=https;AccountName=youtubevideodemo;AccountKey=apipMQSrcSUe1dqvXEuGUZOVSRwjcvTFakZ0gn+R+MmlpqyfB3PEt+1gMr9XT3fOAjiVftY7Wqyy+AStkQD5rA==;EndpointSuffix=core.windows.net";
        // var queueName = "add-weatherdata";
        // var queueClient = new QueueClient(connectionString, queueName);

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Reading from queue");
            var queueMessage = await _queueClient.ReceiveMessageAsync();

            if (queueMessage.Value is not null)
            {
                var weatherData = JsonSerializer.Deserialize<WeatherForecast>(queueMessage.Value.Body);
                _logger.LogInformation("New Message Read: {weatherData}", weatherData);

                await _queueClient.DeleteMessageAsync(queueMessage.Value.MessageId, queueMessage.Value.PopReceipt);
            }

            await Task.Delay(TimeSpan.FromSeconds(10));
        }
    }
}