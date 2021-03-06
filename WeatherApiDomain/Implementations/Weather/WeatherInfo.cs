using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WeatherAPI.Standard.Utilities;
using WeatherApiDomain.Dtos;
using WeatherApiDomain.Interfaces.ExternalServices;
using WeatherApiDomain.Interfaces.Weather;
using WeatherApiDomain.Models;

namespace WeatherApiDomain.Implementations.Weather
{
    public class WeatherInfo: IWeatherInfo
    {       

        private readonly IConsumerServices _services;
        private readonly IConfiguration _appConfig;
        private readonly IStorage _storage;
        private readonly IMessageBroker _messageBroker;
        private readonly ILogger<WeatherInfo> _logger;

        public WeatherInfo(IConsumerServices services, IConfiguration appConfig, IStorage storage, IMessageBroker messageBroker, ILogger<WeatherInfo> logger)
        {
            _services = services;
            _appConfig = appConfig;
            _storage = storage;
            _messageBroker = messageBroker;
            _logger = logger;
          
        }

        public async Task<ResponseWeather<WeatherAPI.Standard.Models.CurrentJsonResponse>> GetAndSaveCurrentWeatherByCity(string cityName)
        {
            var response = await GetCurrentWeather(cityName);
            await SaveCurrentWeather(response);

            if (response.Current.Condition.Text.Contains("rain"))
            {
                //publis event on message broker when current weather includes rain
                await PublishEvent(response);
            }

            return new ResponseWeather<WeatherAPI.Standard.Models.CurrentJsonResponse>() { Data= response , Detail="Data Saved on external service sucessfully", IsSucessFull=true};
        }

        /// <summary>
        /// Get Current Weather By City in WeatherApi
        /// </summary>
        /// <param name="cityName"></param>
        /// <returns></returns>
        private async Task<WeatherAPI.Standard.Models.CurrentJsonResponse> GetCurrentWeather (string cityName)
        {
            //the base uri for api requests
            string _baseUri = _appConfig.GetSection("ApplicationConfig:WeatherApi:BaseUrl").Value;

            //prepare query string for API call
            StringBuilder _queryBuilder = new StringBuilder(_baseUri);
            _queryBuilder.Append("current.json");

            APIHelper.AppendUrlWithQueryParameters(_queryBuilder, new Dictionary<string, object>()
            {
                    { "q", cityName },
                    { "lang", null },
                    { "key", _appConfig.GetSection("ApplicationConfig:WeatherApi:ApiKey").Value}

            }, ArrayDeserialization.Indexed, '&');


            string queryUrl = APIHelper.CleanUrl(_queryBuilder);
            var headers = new Dictionary<string, string>()
            {
                    { "user-agent", "APIMATIC 2.0" },
                    { "accept", "application/json" }
            };

            var response =  await _services.Get<WeatherAPI.Standard.Models.CurrentJsonResponse>(queryUrl, headers);
            _logger.LogTrace($"Data retrived from weather api for {response.Location.Name}");
            return response;
        }

        /// <summary>
        /// Save Current weather on a external storage
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private async Task SaveCurrentWeather(WeatherAPI.Standard.Models.CurrentJsonResponse data )
        {
            WeatherEntity entity = new WeatherEntity()
            {
                Data = JsonConvert.SerializeObject(data),
                RowKey = Guid.NewGuid().ToString(),
                PartitionKey = data.Location.Name
            };
            await _storage.InsertEntityAsync(_appConfig.GetSection("ApplicationConfig:AzureTableStorage:TableName").Value,entity);
            _logger.LogTrace($"Data about current weather from {data.Location.Name} saved on external service at {DateTime.UtcNow}");
        }         

        /// <summary>
        /// Publis message event on messagge broker
        /// </summary>
        /// <returns></returns>
        private async Task PublishEvent(WeatherAPI.Standard.Models.CurrentJsonResponse data)
        {
            WeatherEventMessage weatherEvent = new WeatherEventMessage() { CurrentWeather = data, PublishDate = DateTime.UtcNow, Description = data.Current.Condition.Text, EventType= "rainy day" };
            ServiceBusMessage serviceBusMessage = new ServiceBusMessage(JsonConvert.SerializeObject(weatherEvent));           
            await _messageBroker.PublishMessage(_appConfig.GetSection("ApplicationConfig:AzureServiceBus:WeatherQueue").Value, serviceBusMessage);
            _logger.LogTrace($"Event published to {data.Location.Name} at {DateTime.UtcNow}");
        }

    }
}


