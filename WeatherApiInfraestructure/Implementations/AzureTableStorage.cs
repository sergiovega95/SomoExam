using Azure;
using Azure.Data.Tables;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using WeatherApiDomain.Interfaces.ExternalServices;

namespace WeatherApiInfraestructure.Implementations
{
    public class AzureTableStorage : IStorage
    {

        //TODO: if azure tableStorage is not enough for high demand scenario , use azure cosmos db implementation (more expensive than tablestorage)

        private readonly TableServiceClient _client;        

        public AzureTableStorage(IConfiguration configuration, TableServiceClient client)       
        {
            _client = client;               
        }

        private async Task<TableClient> GetTableClient(string tableName)
        {
            var tableClient = _client.GetTableClient(tableName);
            await tableClient.CreateIfNotExistsAsync();
            return tableClient;
        }             
                

        public async Task InsertEntityAsync (string tableName, ITableEntity entity)
        {
            var tableClient = await GetTableClient(tableName);     
            await tableClient.AddEntityAsync(entity);            
        }        


        public async Task <T> GetAsync<T> (string tableName, string partitionKey, string rowKey) where T : class, ITableEntity, new()
        {
            var tableClient = await GetTableClient(tableName);
            return await tableClient.GetEntityAsync<T>(partitionKey, rowKey);
        }
    }
}
