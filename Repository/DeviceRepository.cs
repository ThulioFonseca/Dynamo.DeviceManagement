using Microsoft.Azure.Cosmos;
using System.ComponentModel;
using System.Net;
using Container = Microsoft.Azure.Cosmos.Container;

namespace Dynamo.DeviceManagement.Repository
{
    public class DeviceRepository<T> : IRepository<T> where T : class
    {
        private Container _container;

        public DeviceRepository(CosmosClient cosmosClient, string databaseId, string containerId)
        {
            InitializeAsync(cosmosClient, databaseId, containerId).GetAwaiter().GetResult();
        }

        // Método assíncrono para inicializar o contêiner
        private async Task InitializeAsync(CosmosClient cosmosClient, string databaseId, string containerId)
        {
            var database = await cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
            _container = await database.Database.CreateContainerIfNotExistsAsync(containerId, "/id");
        }

        public async Task<T?> GetByIdAsync(string id)
        {
            try
            {
                ItemResponse<T> response = await _container.ReadItemAsync<T>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task<IEnumerable<T?>?> GetAllAsync()
        {
            var query = _container.GetItemQueryIterator<T>(new QueryDefinition("SELECT * FROM c"));
            var results = new List<T>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange([.. response]);
            }
            return results;
        }

        public async Task<T?> AddAsync(T entity)
        {
            var response = await _container.CreateItemAsync(entity, new PartitionKey(entity.GetType()?.GetProperty("Id")?.GetValue(entity)?.ToString()));
            return response.Resource;
        }

        public async Task<T?> UpdateAsync(T entity)
        {
            var response = await _container.UpsertItemAsync(entity);
            return response.Resource;
        }

        public async Task DeleteAsync(string id)
        {
            await _container.DeleteItemAsync<T>(id, new PartitionKey(id));
        }


    }

}
