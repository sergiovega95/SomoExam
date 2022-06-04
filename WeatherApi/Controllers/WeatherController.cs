using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
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

        [HttpGet]
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
                return StatusCode(ex.statusCode, ex.Message);
            }
            catch (Exception e)
            {
               return StatusCode(500,e.Message);
            }
                        
        }
    }
}
