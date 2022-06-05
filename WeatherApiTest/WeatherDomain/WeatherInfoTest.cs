using Azure.Data.Tables;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WeatherApiDomain.Implementations.Weather;
using WeatherApiDomain.Interfaces.ExternalServices;
using Xunit;

namespace WeatherApiTest.WeatherDomain
{
    public class WeatherInfoTest
    {
        private readonly Mock<IConsumerServices>_moqServices;
        private readonly Mock<IConfiguration> _moqAppConfig;
        private readonly Mock<IStorage> _moqStorage;
        private readonly Mock<IMessageBroker> _moqMessageBroker;
        private WeatherInfo _weatherInfo;

        public WeatherInfoTest()
        {
            _moqServices = new Mock<IConsumerServices>();
            _moqAppConfig = new Mock<IConfiguration>();
            _moqStorage = new Mock<IStorage>();
            _moqMessageBroker = new Mock<IMessageBroker>();
            _weatherInfo = new WeatherInfo(_moqServices.Object, _moqAppConfig.Object, _moqStorage.Object, _moqMessageBroker.Object);
        }

        //[Fact]
        //public async Task GetAndSaveCurrentWeatherByCity_Successfull_Test()
        //{
        //    //assing
        //    _moqServices.Setup(s => s.Get<WeatherAPI.Standard.Models.CurrentJsonResponse>(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
        //                 .ReturnsAsync(new WeatherAPI.Standard.Models.CurrentJsonResponse(){ Current= new WeatherAPI.Standard.Models.Current(), Location= new WeatherAPI.Standard.Models.Location() });

        //    _moqStorage.Setup(s => s.InsertEntityAsync(It.IsAny<string>(), It.IsAny<ITableEntity>())).Returns(Task.CompletedTask);
        //    _moqMessageBroker.Setup(s => s.PublishMessage(It.IsAny<string>(), It.IsAny<ServiceBusMessage>())).Returns(Task.CompletedTask);           
            
        //    //act                       

        //    //assert
        //} 
    }
}
