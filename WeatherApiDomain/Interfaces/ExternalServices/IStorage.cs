using Azure.Data.Tables;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WeatherApiDomain.Models;

namespace WeatherApiDomain.Interfaces.ExternalServices
{
    public interface IStorage
    {

        Task InsertEntityAsync(string tableName, ITableEntity entity);
        Task<T> GetAsync<T>(string tableName, string partitionKey, string rowKey) where T : class, ITableEntity, new();
    }
}
