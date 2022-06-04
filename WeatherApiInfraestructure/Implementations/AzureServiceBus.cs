using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using WeatherApiDomain.Exceptions;
using WeatherApiDomain.Interfaces.ExternalServices;

namespace WeatherApiInfraestructure.Implementations
{          

    public class AzureServiceBus: IMessageBroker
    {
        private readonly ServiceBusClient _client;
        private ServiceBusSender _sender;

        public AzureServiceBus(IConfiguration configuration, ServiceBusClient serviceBusClient)
        {
            _client = serviceBusClient;            
        }

        public async Task PublishMessage(string queueName, ServiceBusMessage message)
        {
            try
            {
                _sender = _client.CreateSender(queueName);               
                await _sender.SendMessageAsync(message);                
            }           
            finally
            {
                await _sender.DisposeAsync();
            }
        }
    }
}
