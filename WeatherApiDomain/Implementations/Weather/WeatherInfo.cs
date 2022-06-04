using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WeatherAPI.Standard.Utilities;
using WeatherApiDomain.Interfaces.ExternalServices;
using WeatherApiDomain.Interfaces.Weather;
using WeatherApiDomain.Shared.AppConfig;

namespace WeatherApiDomain.Implementations.Weather
{
    public class WeatherInfo: IWeatherInfo
    {       

        private readonly IConsumerServices _services;
        private readonly IConfiguration _appConfig;


        public WeatherInfo(IConsumerServices services, IConfiguration appConfig)
        {
            _services = services;
            _appConfig = appConfig;
          
        }

        public async Task<WeatherAPI.Standard.Models.CurrentJsonResponse> GetAndSaveCurrentWeatherByCity(string cityName)
        {
            return await GetCurrentWeather(cityName);

        }

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

            return await _services.Get<WeatherAPI.Standard.Models.CurrentJsonResponse>(queryUrl, headers);
        }

         

    }
}


