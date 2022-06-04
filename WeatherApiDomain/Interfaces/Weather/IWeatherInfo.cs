using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WeatherApiDomain.Dtos;

namespace WeatherApiDomain.Interfaces.Weather
{
    public interface IWeatherInfo
    {
        Task<ResponseWeather<WeatherAPI.Standard.Models.CurrentJsonResponse>> GetAndSaveCurrentWeatherByCity(string cityName);
    }
}
