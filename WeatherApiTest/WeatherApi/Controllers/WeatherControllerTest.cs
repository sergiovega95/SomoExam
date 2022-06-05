using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WeatherApiDomain.Exceptions;
using WeatherApiDomain.Interfaces.Weather;
using WebApplication1.Controllers;
using Xunit;

namespace WeatherApiTest.WeatherApi.Controllers
{
    public class WeatherControllerTest
    {
        private Mock<ILogger<WeatherController>> _moqLogger;
        private Mock<IWeatherInfo> _moqWeatherInfo;
        private WeatherController _weatherController;

        public WeatherControllerTest()
        {
            _moqLogger = new Mock<ILogger<WeatherController>>();
            _moqWeatherInfo = new Mock<IWeatherInfo>();
            _weatherController = new WeatherController(_moqLogger.Object, _moqWeatherInfo.Object);

        }

        [Fact]
        public async Task GetRealtimeWeatherAsync_Return200Code_Test()
        {
            //asssing
            string cityName = "London";
            _moqWeatherInfo.Setup(s => s.GetAndSaveCurrentWeatherByCity(It.IsAny<string>())).ReturnsAsync(new WeatherApiDomain.Dtos.ResponseWeather<WeatherAPI.Standard.Models.CurrentJsonResponse>() 
            { 
                 Data= new WeatherAPI.Standard.Models.CurrentJsonResponse(),
                 Detail="Successfull",
                 IsSucessFull=true
            });

            //act
            var response = await _weatherController.GetRealtimeWeatherAsync(cityName);
            var data = response as ObjectResult;

            //asert
            Assert.NotNull(data);
            Assert.Equal(200, data.StatusCode);
            
        }

        [Fact]
        public async Task GetRealtimeWeatherAsync_cityNameEmpty_Return400Code_Test()
        {
            //asssing
            string cityName = string.Empty;           

            //act
            var response = await _weatherController.GetRealtimeWeatherAsync(cityName);
            var data = response as ObjectResult;

            //asert
            Assert.NotNull(data);
            Assert.Equal(400, data.StatusCode);

        }

        [Fact]
        public async Task GetRealtimeWeatherAsync_ThrowException_Return500Code_Test()
        {           
            //asssing
            string cityName = "London";
            _moqWeatherInfo.Setup(s => s.GetAndSaveCurrentWeatherByCity(It.IsAny<string>())).ThrowsAsync(new Exception("internal server error"));

            //act
            var response = await _weatherController.GetRealtimeWeatherAsync(cityName);
            var data = response as ObjectResult;

            //asert
            Assert.NotNull(data);
            Assert.Equal(500, data.StatusCode);                     

        }

        [Fact]
        public async Task GetRealtimeWeatherAsync_ThrowExternalServiceException_Test()
        {
            //asssing
            string cityName = "London";
            _moqWeatherInfo.Setup(s => s.GetAndSaveCurrentWeatherByCity(It.IsAny<string>())).ThrowsAsync(new ExternalServiceException("external service error", System.Net.HttpStatusCode.BadRequest));

            //act
            var response = await _weatherController.GetRealtimeWeatherAsync(cityName);
            var data = response as ObjectResult;

            //asert
            Assert.NotNull(data);
            Assert.Equal(400, data.StatusCode);

        }
    }
}
