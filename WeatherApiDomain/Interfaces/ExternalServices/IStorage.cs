using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WeatherApiDomain.Models;

namespace WeatherApiDomain.Interfaces.ExternalServices
{
    public interface IStorage
    {
        Task<WeatherEntity> GetEntityAsync(string partitionKey, string rowKey);
        Task<WeatherEntity> InsertEntityAsync(WeatherEntity entity);
        Task DeleteEntityAsync(string partitionKey, string rowKey);
    }
}
