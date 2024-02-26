using Dynamo.DeviceManagement.Models;
using Dynamo.DeviceManagement.Repository;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        services.AddSingleton(sp =>
        {
            var configuration = sp.GetRequiredService<IConfiguration>();
            var connectionString = configuration["CosmosDBConnectionString"];
            return new CosmosClient(connectionString);
        });

        services.AddScoped<IRepository<Device>>(provider =>
        {
            var cosmosClient = provider.GetRequiredService<CosmosClient>();
            var repository = new DeviceRepository<Device>(cosmosClient, "DynamoDB", "Devices");
            return repository;
        });

    })
    .Build();

host.Run();
