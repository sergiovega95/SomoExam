using Azure.Data.Tables;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
        private readonly Mock<ILogger<WeatherInfo>> _moqLogger;

        public WeatherInfoTest()
        {
            _moqServices = new Mock<IConsumerServices>();
            _moqAppConfig = new Mock<IConfiguration>();
            _moqStorage = new Mock<IStorage>();
            _moqMessageBroker = new Mock<IMessageBroker>();
            _moqLogger = new Mock<ILogger<WeatherInfo>>();
            _weatherInfo = new WeatherInfo(_moqServices.Object, _moqAppConfig.Object, _moqStorage.Object, _moqMessageBroker.Object, _moqLogger.Object);
        }

        [Fact]
        public async Task GetAndSaveCurrentWeatherByCity_Successfull_Test()
        {
            //assing
            string cityName = "London";

            _moqServices.Setup(s => s.Get<WeatherAPI.Standard.Models.CurrentJsonResponse>(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
                         .ReturnsAsync(new WeatherAPI.Standard.Models.CurrentJsonResponse() { Current = new WeatherAPI.Standard.Models.Current() {  Condition= new WeatherAPI.Standard.Models.Condition() { Text="rain"} }, Location = new WeatherAPI.Standard.Models.Location() });

            _moqStorage.Setup(s => s.InsertEntityAsync(It.IsAny<string>(), It.IsAny<ITableEntity>())).Returns(Task.CompletedTask);
            _moqMessageBroker.Setup(s => s.PublishMessage(It.IsAny<string>(), It.IsAny<ServiceBusMessage>())).Returns(Task.CompletedTask);

            _moqMessageBroker.Setup(s => s.PublishMessage(It.IsAny<string>(), It.IsAny<ServiceBusMessage>())).Returns(Task.CompletedTask);

            var webapiBaseurlMock = new Mock<IConfigurationSection>();
            webapiBaseurlMock.Setup(s => s.Value).Returns("https://api.weatherapi.com/v1/");

            var apiKeyMock = new Mock<IConfigurationSection>();
            apiKeyMock.Setup(s => s.Value).Returns("fakeapikey");

            var azureTableNameMock = new Mock<IConfigurationSection>();
            azureTableNameMock.Setup(s => s.Value).Returns("WeatherTable");

            var weatherQueueMock = new Mock<IConfigurationSection>();
            weatherQueueMock.Setup(s => s.Value).Returns("WeatherQueue");

            _moqAppConfig.Setup(s => s.GetSection("ApplicationConfig:WeatherApi:BaseUrl")).Returns(webapiBaseurlMock.Object);
            _moqAppConfig.Setup(s => s.GetSection("ApplicationConfig:WeatherApi:ApiKey")).Returns(apiKeyMock.Object);
            _moqAppConfig.Setup(s => s.GetSection("ApplicationConfig:AzureTableStorage:TableName")).Returns(azureTableNameMock.Object);
            _moqAppConfig.Setup(s => s.GetSection("ApplicationConfig:AzureServiceBus:WeatherQueue")).Returns(weatherQueueMock.Object);

            //act                       
            var response = await _weatherInfo.GetAndSaveCurrentWeatherByCity(cityName);

            //assert
            Assert.NotNull(response);
            Assert.True(response.IsSucessFull);
        }
    }
}
