using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApiDomain.Interfaces.Weather
{
    public interface IWeatherInfo
    {
        Task<WeatherAPI.Standard.Models.CurrentJsonResponse> GetAndSaveCurrentWeatherByCity(string cityName);
    }
}
