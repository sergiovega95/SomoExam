using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Net;
using System.Threading.Tasks;
using WeatherApiDomain.Dtos;
using WeatherApiDomain.Exceptions;
using WeatherApiDomain.Interfaces.Weather;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherController : ControllerBase
    {        

        private readonly ILogger<WeatherController> _logger;
        private readonly IWeatherInfo _weatherInfo;

        public WeatherController(ILogger<WeatherController> logger, IWeatherInfo weatherInfo)
        {
            _logger = logger;
            _weatherInfo = weatherInfo;
        }



        /// <summary>
        /// Get Current weather of a city
        /// </summary>
        /// <param name="cityName">city name of the city you want to know current weather Example:London</param>
        /// <returns></returns>
        [SwaggerResponse(200, "Current Weather of the city", typeof(ResponseWeather<WeatherAPI.Standard.Models.CurrentJsonResponse>))]
        [SwaggerResponse(500,"Internal Server Error", Type = typeof(string))]
        [HttpGet("{cityName}")]
        public async Task<IActionResult> GetRealtimeWeatherAsync(string cityName)
        {          
            try
            {
                if (string.IsNullOrEmpty(cityName))
                {
                   return StatusCode(400, "cityName is required");
                }

                var response = await _weatherInfo.GetAndSaveCurrentWeatherByCity(cityName);
                return StatusCode(200, response);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex.Message, ex);
                return StatusCode(ex.statusCode, new ResponseWeather<object>(){Error=ex.Message,Detail="ExternalService Error"});
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
                return StatusCode(500,e.Message);
            }
                        
        }
    }
}
