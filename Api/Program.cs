using Api;
using Azure.Identity;
using Azure.Storage.Queues;
using Microsoft.Extensions.Azure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAzureClients(clientBuilder =>
{
    clientBuilder.AddClient<QueueClient, QueueClientOptions>((_, _, _) =>
    {
        var connectionString = "DefaultEndpointsProtocol=https;AccountName=youtubevideodemo;AccountKey=apipMQSrcSUe1dqvXEuGUZOVSRwjcvTFakZ0gn+R+MmlpqyfB3PEt+1gMr9XT3fOAjiVftY7Wqyy+AStkQD5rA==;EndpointSuffix=core.windows.net";
        var queueName = "add-weatherdata";
        var queueClient = new QueueClient(connectionString, queueName);
        return queueClient;
    });
});

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddHostedService<WeatherDataService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
