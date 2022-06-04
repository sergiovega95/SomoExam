using Azure.Data.Tables;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WeatherApiDomain.Interfaces.ExternalServices;
using WeatherApiDomain.Models;

namespace WeatherApiInfraestructure.Implementations
{
    public class AzureTableStorage : IStorage
    {
        private const string TableName = "Item";
        private readonly IConfiguration _configuration;

        public AzureTableStorage(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private async Task<TableClient> GetTableClient()
        {
            var serviceClient = new TableServiceClient(_configuration["ApplicationConfig:AzureStorage:StorageConnectionString"]);
            var tableClient = serviceClient.GetTableClient(TableName);
            await tableClient.CreateIfNotExistsAsync();
            return tableClient;
        }


        public async Task DeleteEntityAsync(string partitionKey, string rowKey)
        {
            var tableClient = await GetTableClient();
            await tableClient.DeleteEntityAsync(partitionKey, rowKey);
        }

        public async Task<WeatherEntity> GetEntityAsync(string partitionKey, string rowKey) 
        {
            var tableClient = await GetTableClient();
            return await tableClient.GetEntityAsync<WeatherEntity>( partitionKey,  rowKey);
        }

        public async Task<WeatherEntity> InsertEntityAsync (WeatherEntity entity)
        {
            var tableClient = await GetTableClient();
            await tableClient.UpsertEntityAsync(entity);
            return entity;
        }
    }
}
