using Azure;
using Azure.Data.Tables;
using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherApiDomain.Models
{
    public class WeatherEntity : WeatherAPI.Standard.Models.CurrentJsonResponse,ITableEntity
    {
        public string PartitionKey { get; set; }

        public string RowKey { get; set; }

        public DateTimeOffset? Timestamp { get; set; }
        
        public ETag ETag { get; set; }
    }
}
